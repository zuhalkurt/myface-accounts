import React, {ReactNode} from "react";
import {Header, Nav} from "../../Components/Header/Header";
import {Footer} from "../../Components/Footer/Footer";
import "./Page.scss";

interface PageProps {
    children: ReactNode;
    containerClassName?: string;
}

export function Page(props: PageProps): JSX.Element {
    return ( 
        <div className={"page"}>
            <Header/>
            <Nav/>
            <main className={`main ${props.containerClassName}`}>
                {props.children}
            </main>
            <Footer/>
        </div>
    );
}