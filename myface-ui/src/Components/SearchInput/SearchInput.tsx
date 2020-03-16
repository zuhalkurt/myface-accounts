import React, {FormEvent} from "react";
import "./SearchInput.scss";

interface SearchInputProps {
    searchTerm: string;
    updateSearchTerm: (searchTerm: string) => void
}

export function SearchInput(props: SearchInputProps): JSX.Element {
    function preventDefault(event: FormEvent) {
        event.preventDefault();
        // search term is already synced with the parent, so nothing else needs to happen here.
    }
    
    return (
        <form className="search-form" onSubmit={preventDefault}>
            <label className="form-label">
                Search
                <input className="form-input" type="text" 
                       value={props.searchTerm} 
                       onChange={event => props.updateSearchTerm(event.target.value)}/>
            </label>
        </form>
    );
}