import React, {ReactNodeArray} from "react";
import "./Grid.scss";

interface GridProps {
    children: ReactNodeArray
}

export function Grid(props: GridProps): JSX.Element {
    return (
        <ol className="grid">
            {props.children.map((child, index) => <li className="list-item" key={index}>{child}</li>)}    
        </ol>
    );
}