import logo from './logo.svg';
import React from 'react';
import '../node_modules/bootstrap/dist/css/bootstrap.min.css';
import './App.css';
import CreateUserAccount from "./components/createuseraccount.component";

function App() {
  return (
    <div className="App">
       {/* Default code commented out for testing */}
      {/*<header className="App-header">
       
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
        {/* CreateUserAccount component 
          1. Set up HTML template code
          TODO:
          2. Add CSS
          3. Connect to backend */}
        <CreateUserAccount />
      </div>
    </div>
  );
}

export default App;
