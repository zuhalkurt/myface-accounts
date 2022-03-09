﻿export interface ListResponse<T> {
    items: T[];
    totalNumberOfItems: number;
    page: number;
    nextPage: string;
    previousPage: string;
}

export interface User {
    id: number;
    firstName: string;
    lastName: string;
    displayName: string;
    username: string;
    email: string;
    profileImageUrl: string;
    coverImageUrl: string;
}

export interface Interaction {
    id: number;
    user: User;
    type: string;
    date: string;
}

export interface Post {
    id: number;
    message: string;
    imageUrl: string;
    postedAt: string;
    postedBy: User;
    likes: Interaction[];
    dislikes: Interaction[];
}

export interface NewPost {
    message: string;
    imageUrl: string;
    userId: number;
}

export interface NewUser {
    firstName: string;
    lastName: string;
    username: string;
    email: string;
    password : string;
}

function getAuthHeader(username: string, password: string)
{
    return `Basic ${btoa(`${username}:${password}`)}`;
}

export async function fetchUsers(searchTerm: string, page: number, pageSize: number): Promise<ListResponse<User>> {
    const response = await fetch(`https://localhost:5001/users?search=${searchTerm}&page=${page}&pageSize=${pageSize}`);
    return await response.json();
}

export async function fetchUser(userId: string | number): Promise<User> {
    const response = await fetch(`https://localhost:5001/users/${userId}`);
    return await response.json();
}

export async function fetchPosts(page: number, pageSize: number): Promise<ListResponse<Post>> {
    const response = await fetch(`https://localhost:5001/feed?page=${page}&pageSize=${pageSize}`);
    return await response.json();
}

export async function fetchPostsForUser(page: number, pageSize: number, userId: string | number) {
    const response = await fetch(`https://localhost:5001/feed?page=${page}&pageSize=${pageSize}&postedBy=${userId}`);
    return await response.json();
}

export async function fetchPostsLikedBy(page: number, pageSize: number, userId: string | number) {
    const response = await fetch(`https://localhost:5001/feed?page=${page}&pageSize=${pageSize}&likedBy=${userId}`);
    return await response.json();
}

export async function fetchPostsDislikedBy(page: number, pageSize: number, userId: string | number) {
    const response = await fetch(`https://localhost:5001/feed?page=${page}&pageSize=${pageSize}&dislikedBy=${userId}`);
    return await response.json();
}

export async function createPost(newPost: NewPost, username: string, password: string) {
    const response = await fetch(`https://localhost:5001/posts/create`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            "Authorization": getAuthHeader(username, password),
        },
        body: JSON.stringify(newPost),
    });
    
    if (!response.ok) {
        throw new Error(await response.json())
    }
}
export async function createUser(newUser: NewUser) {
    const response = await fetch(`https://localhost:5001/users/create`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(newUser),
    });
    
    if (!response.ok) {
        throw new Error(await response.json())
    }
}
