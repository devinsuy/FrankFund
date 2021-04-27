import React, { useState, useEffect } from 'react';
import '../node_modules/bootstrap/dist/css/bootstrap.min.css';
import './App.css';
import axios from 'axios';
import swal from 'sweetalert';
import Swal from 'sweetalert2'
import Badge from '@material-ui/core/Badge';
import { makeStyles } from "@material-ui/core/styles";
import { BrowserRouter as Router, Switch, Redirect, Route, Link, } from "react-router-dom";
import { createBrowserHistory } from 'history';

// Imported React Components
import CreateUserAccount from "./components/createuseraccount.component";
import SettingsUserAccount from "./components/settingsuseraccount.component";
import LoginComponent from "./components/loginuseraccount.component";
import LandingComponent from "./components/landing.components";
import TransactionLog from "./components/transactionslog.component";
import SavingsGoalsLog from "./components/savingsgoalslog.component";
import ImageUploadDemo from './components/temp/imageuploaddemo.component';
import DashboardComponent from './components/Dashboard/dashboard.component';
import Trends from './components/trends.component';
//import loginuseraccountComponent from './components/loginuseraccount.component';
import SubscriptionsLog from './components/subscriptionslog.component';

function logout() {
    const user = JSON.parse(localStorage.getItem('user'));
    let loading = true;
    while (loading) {
        Swal.fire({
            title: 'Logging out...',
            allowOutsideClick: false,
            onBeforeOpen: () => { Swal.showLoading() },
            onAfterClose() {
                Swal.hideLoading()
            }
        });
        // Calls axios function to post the JSON data for PATCH request at API endpoint
        // Need to add current user state to delete in accID=
        axios({
            method: "delete",
            url: "/api/session/jwt=" + user.JWTToken + "&apikey=bd0eecf7cf275751a421a6101272f559b0391fa0",
            //data: json,
            //headers: {
            //    'accept': 'application/json',
            //    'content-type': 'application/json'
            //}
        })
            .then((res) => {
                console.log(res);
                localStorage.removeItem("user");
                Swal.close()
                swal("Success!", "You have successfully logged out!", "success");
                window.location.reload(false);
            })
            .catch((err) => {
                swal("Error!", "An error has occured.", "error");
                throw err;
            })
        // Exit loading loop
        loading = false;
    }
}

const useStyles = makeStyles((theme) => ({
    badge: {
        fontSize: 12,
        backgroundColor: "#729b79",
        color: "white",
        marginTop: '2px',
        marginRight: '-4px'
    }
}));



function App() {
    const isLoggedIn = window.localStorage.getItem("user") ? true : false;
    const user = JSON.parse(localStorage.getItem("user"));
    const classes = useStyles();
    function subscriptionCount() {
        if (isLoggedIn) {
            axios({
                method: "get",
                url: "/api/account/user=" + user.AccountUsername + "/Subscriptions/count&apikey=bd0eecf7cf275751a421a6101272f559b0391fa0",
            })
                .then((res) => {
                    console.log(res);
                })
                .catch((err) => {
                    throw err;
                })
            return 1;
        }
    }
    const subsCount = subscriptionCount();

    return (<Router history={createBrowserHistory}>
      <div className="App">
      {/* Landing Page and CreateUserAccount component 
          1. Set up HTML template code
          2. Add CSS
          3. Connect to backend */}
      <nav className="navbar navbar-expand-lg navbar-light fixed-top">
        <div className="container">
          <div class="logo-image">
            <img src="https://lh3.googleusercontent.com/yMIRhv9YgrNwhWYatcVfb8l_BdoYdhYOy7jTPW42BaShPMQqmTxTKXM8XlX9pLZxzBes4HN1HpHIk52wW5drkxvwrQJ4G5_Q6pcBnOn4L_9zILLM41b--QxoUSVbHHVQKpW4Mfks4Q=w2400" class="img-fluid"/>
          </div>
          <Link className="navbar-brand" to={"/"}>FrankFund</Link>
          {/* {!isLoggedIn && <Link className="navbar-brand" to={"/"}>FrankFund</Link>}
          {isLoggedIn && <Link className="navbar-brand" to={"/dashboard"}>FrankFund</Link>}*/}
          <div class="collapse navbar-collapse" id="navbarNavTransaction">
            <ul class="navbar-nav">
              <li class="nav-item">
                {isLoggedIn && <Link className="nav-link" to={"/transactions"}>Transactions<span class="sr-only">(current)</span></Link>}
              </li>
              <li class="nav-item">
                {isLoggedIn && <Link className="nav-link" to={"/goals"}>Goals</Link>}     
              </li>
              <li class="nav-item">
                {isLoggedIn && <Badge badgeContent={subsCount} color="secondary" overlap="circle" classes={{ badge: classes.badge }}>
                    <Link className="nav-link" to={"/subscriptions"}>Subscriptions</Link>
                </Badge>}
              </li>
              <li class="nav-item">
                {isLoggedIn && <Link className="nav-link" to={"/trends"}>Trends</Link>}
              </li>
              <li class="nav-item">
                {isLoggedIn && <Link className="nav-link" to={"/receipts/imageupload"}>ImageUpload Demo</Link>}   
              </li>
            </ul>
          </div>
          <div className="collapse navbar-collapse" id="navbarTogglerDemo02">
            <ul className="navbar-nav ml-auto">
              <li className="nav-item">
                {!isLoggedIn && <Link className="nav-link" to={"/login"}>Login</Link>}
              </li>
              <li className="nav-item">
                {!isLoggedIn && <Link className="nav-link" to={"/create-user-account"}>Sign up</Link>}
              </li>
              <li className="nav-item">
                {isLoggedIn && <Link className="nav-link" to={"/account-settings"}>Settings</Link>}
              </li>
              <li className="nav-item">
                {isLoggedIn && <Link className="nav-link" to={"/"} onClick={logout}>Logout</Link>}
              </li>
            </ul>
          </div>
        </div>
      </nav>

      <div className="outer">
        <div className="inner">
          <Switch>
            <Route exact path="/" component={() => isLoggedIn ? <DashboardComponent /> : <LandingComponent />} />
            <Route exact path='/landing' component={LandingComponent} />
            <Route exact path='/dashboard' component={DashboardComponent} />
            <Route path="/create-user-account" component={CreateUserAccount} />
            <Route path="/account-settings" component={SettingsUserAccount} />
            <Route path="/login" component={LoginComponent} />
            <Route path="/transactions-log" component={TransactionLog} />
            <Route path="/transactions" component={() => isLoggedIn ? <TransactionLog /> : <LoginComponent/> } />
            <Route path="/goals" component={() => isLoggedIn ? <SavingsGoalsLog /> : <LoginComponent/> } />
            <Route path="/subscriptions" component={() => isLoggedIn ? <SubscriptionsLog /> : <LoginComponent/> } />
            <Route path="/trends" component={() => isLoggedIn ? <Trends /> : <LoginComponent/> } />
            <Route path="/receipts/imageupload" component={ImageUploadDemo} />
          </Switch>
        </div>
      </div>
    </div></Router>
  );
}

export default App;
