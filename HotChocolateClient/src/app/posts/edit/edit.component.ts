import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from "@angular/router";
import { Subject } from "rxjs";
import { takeUntil } from "rxjs/operators";
import { Post } from "../../../generated/graphql";
import { Apollo } from "apollo-angular";
import gql from "graphql-tag";

export const getQuery = gql`query($id : Uuid) {
  posts(where: { id: $id }) {
    nodes {
      id
      title
      content
      comments {
        id
        posterName
        content
      }
    }
  }
}`

const saveMutation = gql`mutation($post:PostInput) {
  savePost(post:$post) {
    id
  }
}`

@Component({
  selector: 'app-edit',
  templateUrl: './edit.component.html',
  styleUrls: ['./edit.component.scss']
})
export class EditComponent implements OnInit, OnDestroy {

  public id: string;
  public post: Post;

  private unsub = new Subject<boolean>();

  constructor(private route: ActivatedRoute, private apollo: Apollo, private router: Router) {
  }

  ngOnInit(): void {
    this.route.queryParamMap
      .pipe(takeUntil(this.unsub))
      .subscribe( (result: any) => {
        this.id = result.params['id'];
        if (this.id === 'new') {
          this.post = { id: null };
        } else {
          this.post = null;
          this.apollo.query({ query: getQuery, variables: { id: this.id }}).subscribe((r: any) => {
            this.post = r.data.posts.nodes[0];
            delete(this.post.comments);
            delete(this.post.__typename);
          });
        }
      });
  }

  ngOnDestroy(): void {
    this.unsub.next(true);
    this.unsub.complete();
  }

  save() {
    console.info(this.post);
    this.apollo.mutate({ mutation: saveMutation, variables: { post: this.post }}).subscribe(() => {
      this.router.navigateByUrl('/');
    });
  }

}
