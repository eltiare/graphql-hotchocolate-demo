import gql from 'graphql-tag';
export type Maybe<T> = T | null;
export type Exact<T extends { [key: string]: any }> = { [K in keyof T]: T[K] };
/** All built-in and custom scalars, mapped to their actual values */
export type Scalars = {
  ID: string;
  String: string;
  Boolean: boolean;
  Int: number;
  Float: number;
  PaginationAmount: any;
  Uuid: any;
  /** The multiplier path scalar represents a valid GraphQL multiplier path string. */
  MultiplierPath: any;
};

export type Comment = {
  __typename?: 'Comment';
  content?: Maybe<Scalars['String']>;
  id?: Maybe<Scalars['Uuid']>;
  posterName?: Maybe<Scalars['String']>;
  postId: Scalars['Uuid'];
};

export type CommentInput = {
  content?: Maybe<Scalars['String']>;
  id?: Maybe<Scalars['Uuid']>;
  posterName?: Maybe<Scalars['String']>;
  postId: Scalars['Uuid'];
};


export type Mutation = {
  __typename?: 'Mutation';
  deleteComment: Scalars['Boolean'];
  deletePost: Scalars['Boolean'];
  saveComment?: Maybe<Comment>;
  savePost?: Maybe<Post>;
};


export type MutationDeleteCommentArgs = {
  id: Scalars['Uuid'];
};


export type MutationDeletePostArgs = {
  id: Scalars['Uuid'];
};


export type MutationSaveCommentArgs = {
  comment?: Maybe<CommentInput>;
};


export type MutationSavePostArgs = {
  post?: Maybe<PostInput>;
};

/** Information about pagination in a connection. */
export type PageInfo = {
  __typename?: 'PageInfo';
  /** When paginating forwards, the cursor to continue. */
  endCursor?: Maybe<Scalars['String']>;
  /** Indicates whether more edges exist following the set defined by the clients arguments. */
  hasNextPage: Scalars['Boolean'];
  /** Indicates whether more edges exist prior the set defined by the clients arguments. */
  hasPreviousPage: Scalars['Boolean'];
  /** When paginating backwards, the cursor to continue. */
  startCursor?: Maybe<Scalars['String']>;
};


export type Post = {
  __typename?: 'Post';
  comments?: Maybe<Array<Maybe<Comment>>>;
  content?: Maybe<Scalars['String']>;
  id?: Maybe<Scalars['Uuid']>;
  title?: Maybe<Scalars['String']>;
};

/** A connection to a list of items. */
export type PostConnection = {
  __typename?: 'PostConnection';
  /** A list of edges. */
  edges?: Maybe<Array<PostEdge>>;
  /** A flattened list of the nodes. */
  nodes?: Maybe<Array<Maybe<Post>>>;
  /** Information to aid in pagination. */
  pageInfo: PageInfo;
  totalCount: Scalars['Int'];
};

/** An edge in a connection. */
export type PostEdge = {
  __typename?: 'PostEdge';
  /** A cursor for use in pagination. */
  cursor: Scalars['String'];
  /** The item at the end of the edge. */
  node?: Maybe<Post>;
};

export type PostFilter = {
  AND?: Maybe<Array<PostFilter>>;
  content?: Maybe<Scalars['String']>;
  content_contains?: Maybe<Scalars['String']>;
  content_ends_with?: Maybe<Scalars['String']>;
  content_in?: Maybe<Array<Maybe<Scalars['String']>>>;
  content_not?: Maybe<Scalars['String']>;
  content_not_contains?: Maybe<Scalars['String']>;
  content_not_ends_with?: Maybe<Scalars['String']>;
  content_not_in?: Maybe<Array<Maybe<Scalars['String']>>>;
  content_not_starts_with?: Maybe<Scalars['String']>;
  content_starts_with?: Maybe<Scalars['String']>;
  id?: Maybe<Scalars['Uuid']>;
  id_gt?: Maybe<Scalars['Uuid']>;
  id_gte?: Maybe<Scalars['Uuid']>;
  id_in?: Maybe<Array<Maybe<Scalars['Uuid']>>>;
  id_lt?: Maybe<Scalars['Uuid']>;
  id_lte?: Maybe<Scalars['Uuid']>;
  id_not?: Maybe<Scalars['Uuid']>;
  id_not_gt?: Maybe<Scalars['Uuid']>;
  id_not_gte?: Maybe<Scalars['Uuid']>;
  id_not_in?: Maybe<Array<Maybe<Scalars['Uuid']>>>;
  id_not_lt?: Maybe<Scalars['Uuid']>;
  id_not_lte?: Maybe<Scalars['Uuid']>;
  OR?: Maybe<Array<PostFilter>>;
  title?: Maybe<Scalars['String']>;
  title_contains?: Maybe<Scalars['String']>;
  title_ends_with?: Maybe<Scalars['String']>;
  title_in?: Maybe<Array<Maybe<Scalars['String']>>>;
  title_not?: Maybe<Scalars['String']>;
  title_not_contains?: Maybe<Scalars['String']>;
  title_not_ends_with?: Maybe<Scalars['String']>;
  title_not_in?: Maybe<Array<Maybe<Scalars['String']>>>;
  title_not_starts_with?: Maybe<Scalars['String']>;
  title_starts_with?: Maybe<Scalars['String']>;
};

export type PostInput = {
  comments?: Maybe<Array<Maybe<CommentInput>>>;
  content?: Maybe<Scalars['String']>;
  id?: Maybe<Scalars['Uuid']>;
  title?: Maybe<Scalars['String']>;
};

export type Query = {
  __typename?: 'Query';
  posts?: Maybe<PostConnection>;
};


export type QueryPostsArgs = {
  after?: Maybe<Scalars['String']>;
  before?: Maybe<Scalars['String']>;
  first?: Maybe<Scalars['PaginationAmount']>;
  last?: Maybe<Scalars['PaginationAmount']>;
  where?: Maybe<PostFilter>;
};
