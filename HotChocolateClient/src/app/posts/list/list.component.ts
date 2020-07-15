import { Component, OnInit } from '@angular/core';
import { Post } from "../../../generated/graphql";
import { Apollo } from "apollo-angular";
import gql from "graphql-tag";

const listQuery = gql`query {
  posts {
    nodes {
      id
      title
    }
  }
}`;

const deleteMutation = gql`mutation($id: Uuid!) {
  deletePost(id: $id)
}`

@Component({
  selector: 'app-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.scss']
})
export class ListComponent implements OnInit {

  public posts: Post[];

  constructor(private apollo: Apollo) {

  }

  ngOnInit(): void {
    this.loadPosts();
  }

  delete(post: Post) {
    if (confirm(`Are you sure you want to delete ${post.title}?`)) {
      this.apollo.mutate({ mutation: deleteMutation, variables: { id: post.id } }).subscribe( r => {
        this.loadPosts();
      })
    }
  }

  private loadPosts() {
    this.apollo.query({ query: listQuery, fetchPolicy: 'network-only' }).subscribe((r: any) => {
      this.posts = r.data.posts.nodes;
    });
  }

}
