import React, { useState, useEffect } from 'react';
import '../node_modules/bootstrap/dist/css/bootstrap.min.css';
import './App.css';
import { BrowserRouter as Router, Switch,  Redirect, Route, Link,  } from "react-router-dom";

// Imported React Components
import CreateUserAccount from "./components/createuseraccount.component";
import SettingsUserAccount from "./components/settingsuseraccount.component";
import LoginComponent from "./components/loginuseraccount.component";
import LandingComponent from "./components/landing.components";
import TransactionLog from "./components/transactionslog.component";
import TransactionDetail from "./components/transactiondetail.component";
import CreateTransaction from "./components/createtransaction.component";
import SavingsGoalsLog from "./components/savinggoals/savingsgoalslog.component";
import ModifyTransaction from './components/updatetransaction.component';
import DeleteTransaction from './components/deletetransaction.component';
import ImageUploadDemo from './components/temp/imageuploaddemo.component';


function App() {
    return (<Router>
      <div className="App">
      {/* Landing Page and CreateUserAccount component 
          1. Set up HTML template code
          TODO:
          2. Add CSS
          3. Connect to backend */}
      <nav className="navbar navbar-expand-lg navbar-light fixed-top">
        <div className="container">
          <Link className="navbar-brand" to={"/"}>FrankFund</Link>
          <div class="collapse navbar-collapse" id="navbarNavTransaction">
            <ul class="navbar-nav">
              <li class="nav-item">
                <a class="nav-link" href="/transactions/KennethTran">Transactions<span class="sr-only">(current)</span></a>
              </li>
              <li class="nav-item">
                { /* TODO: Button click should add current logged in user to the route /goals/{user} */ }
                <a class="nav-link" href="/goals/DevinSuy">SavingsGoals</a>     
              </li>
              <li class="nav-item">
                <a class="nav-link" href="/receipts/imageupload">ImageUpload Demo</a>     
              </li>
            </ul>
          </div>
          <div className="collapse navbar-collapse" id="navbarTogglerDemo02">
            <ul className="navbar-nav ml-auto">
              <li className="nav-item">
                <Link className="nav-link" to={"/login"}>Login</Link>
              </li>
              <li className="nav-item">
                <Link className="nav-link" to={"/create-user-account"}>Sign up</Link>
              </li>
              <li className="nav-item">
                <Link className="nav-link" to={"/account-settings"}>Settings</Link>
              </li>
            </ul>
          </div>
        </div>
      </nav>

      <div className="outer">
        <div className="inner">
          <Switch>
            <Route exact path='/' component={LandingComponent} />
            <Route path="/create-user-account" component={CreateUserAccount} />
            <Route path="/account-settings" component={SettingsUserAccount} />
            <Route path="/login" component={LoginComponent} />
            <Route path="/transactions-detail" component={TransactionDetail} />
            <Route path="/transactions-log" component={TransactionLog} />
            <Route path="/create-transaction" component={CreateTransaction} />
            <Route path="/modify-transaction" component={ModifyTransaction} />
            <Route path="/delete-transaction" component={DeleteTransaction} />
            { /* Example: frankfund.appspot.com/goals/DevinSuy */ }
            <Route path="/goals/:user" component={SavingsGoalsLog} />
            <Route path="/transactions/:user" component={TransactionLog} />       
            <Route path="/receipts/imageupload" component={ImageUploadDemo} />
          </Switch>
        </div>
      </div>
    </div></Router>
  );
}

export default App;
