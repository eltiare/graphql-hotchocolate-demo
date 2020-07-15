import { Component, OnDestroy, OnInit } from '@angular/core';
import { takeUntil } from "rxjs/operators";
import { getQuery } from "../edit/edit.component";
import { ActivatedRoute, Router } from "@angular/router";
import { Subject } from "rxjs";
import { Post } from "../../../generated/graphql";
import { Apollo } from "apollo-angular";

@Component({
  selector: 'app-show',
  templateUrl: './show.component.html',
  styleUrls: ['./show.component.scss']
})
export class ShowComponent implements OnInit, OnDestroy {

  public post: Post;
  private unsub = new Subject<boolean>();

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private apollo: Apollo
  ) { }

  ngOnInit(): void {
    this.route.queryParamMap
      .pipe(takeUntil(this.unsub))
      .subscribe( (result: any) => {
        const id = result.params['id'];
        this.post = null;
        this.apollo.query({ query: getQuery, variables: { id }}).subscribe((r: any) => {
          this.post = r.data.posts.nodes[0];
        });
      });
  }

  ngOnDestroy(): void {
    this.unsub.next(true);
    this.unsub.complete();
  }

}
