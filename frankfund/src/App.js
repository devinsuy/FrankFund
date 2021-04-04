import logo from './logo.svg';
import React from 'react';
import '../node_modules/bootstrap/dist/css/bootstrap.min.css';
import './App.css';
import { BrowserRouter as Router, Switch, Route, Link } from "react-router-dom";

import CreateUserAccount from "./components/createuseraccount.component";

function App() {
    return (<Router>
    {/* Default code commented out for testing */ }
    {/*<div className="App">
      <header className="App-header">
       
        <img src={logo} className="App-logo" alt="logo" />
        <p>
          Edit <code>src/App.js</code> and save to reload.
        </p>
        <a
          className="App-link"
          href="https://reactjs.org"
          target="_blank"
          rel="noopener noreferrer"
        >
          Learn React
        </a> 
      </header>*/}

      <div className="App">
      {/* Landing Page and CreateUserAccount component 
          1. Set up HTML template code
          TODO:
          2. Add CSS
          3. Connect to backend */}
      <nav className="navbar navbar-expand-lg navbar-light fixed-top">
        <div className="container">
          <Link className="navbar-brand" to={"/create-user-account"}>FrankFund</Link>
          <div className="collapse navbar-collapse" id="navbarTogglerDemo02">
            <ul className="navbar-nav ml-auto">
              <li className="nav-item">
                <Link className="nav-link" to={"/create-user-account"}>Sign up</Link>
              </li>
            </ul>
          </div>
        </div>
      </nav>

      <div className="outer">
        <div className="inner">
          <Switch>
            <Route exact path='/' component={CreateUserAccount} />
            <Route path="/create-user-account" component={CreateUserAccount} />
          </Switch>
        </div>
      </div>
    </div></Router>
  );
}

export default App;
