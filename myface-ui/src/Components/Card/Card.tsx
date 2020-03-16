import React, {ReactNode} from "react";
import "./card.scss";

interface CardProps {
    children: ReactNode;
}

export function Card(props: CardProps): JSX.Element {
    return (
        <div className="card">
            {props.children}
        </div>
    );
}