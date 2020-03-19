import React, {createContext, ReactNode, useState} from "react";

export const LoginContext = createContext({
    isLoggedIn: false,
    username: "",
    password: "",
    isAdmin: false,
    logIn: (username: string, password: string) => {},
    logOut: () => {},
});

interface LoginManagerProps {
    children: ReactNode
}

export function LoginManager(props: LoginManagerProps): JSX.Element {
    const [loggedIn, setLoggedIn] = useState(false);
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    
    
    function logIn(username: string, password: string) {
        setLoggedIn(true);
        setUsername(username);
        setPassword(password);
    }
    
    function logOut() {
        setLoggedIn(false);
        setUsername("");
        setPassword("");
    }
    
    const context = {
        isLoggedIn: loggedIn,
        username: username,
        password: password,
        isAdmin: loggedIn,
        logIn: logIn,
        logOut: logOut,
    };
    
    return (
        <LoginContext.Provider value={context}>
            {props.children}
        </LoginContext.Provider>
    );
}