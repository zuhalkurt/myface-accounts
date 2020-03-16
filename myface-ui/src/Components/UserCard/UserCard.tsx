import React from "react";
import {User} from "../../Api/apiClient";
import {Card} from "../Card/Card";
import "./UserCard.scss";
import {Link} from "react-router-dom";

interface UserCardProps {
    user: User;
}

export function UserCard(props: UserCardProps) {
    return (
        <Card>
            <Link className="user-card" to={`/users/${props.user.id}`}>
                <img className="profile-image" src={props.user.profileImageUrl} alt=""/>
                <div className="user-details">
                    <div className="name">{props.user.displayName}</div>
                    <div className="username">{props.user.username}</div>
                </div>
            </Link>
        </Card>
    );
}