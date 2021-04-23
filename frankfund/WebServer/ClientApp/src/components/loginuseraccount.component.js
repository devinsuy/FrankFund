// JavaScript source code for User Account Login React Component

import React, { Component } from "react";
import axios from 'axios';
import swal from 'sweetalert';
import Swal from 'sweetalert2'
import { withRouter } from "react-router-dom";

class LoginUserAccount extends Component {

    constructor(props) {
        super(props);

        this.login = this.login.bind(this);
    }

    login(e) {
        e.preventDefault();
        const form = document.querySelector("form");
        let data = new FormData(form);
        var object = {};
        data.forEach(function (value, key) {
            object[key] = value;
        });
        var json = JSON.stringify(object);
        console.log(json);

        let loading = true;
        while (loading) {
            Swal.fire({
                title: 'Logging in...',
                allowOutsideClick: false,
                onBeforeOpen: () => { Swal.showLoading() },
                onAfterClose() {
                    Swal.hideLoading()
                }
            });
            // Calls axios function to post the JSON data for POST request at API endpoint
            axios({
                method: "post",
                url: "/api/session/create&apikey=bd0eecf7cf275751a421a6101272f559b0391fa0",
                data: json,
                headers: {
                    'accept': 'application/json',
                    'content-type': 'application/json'
                }
            })
                .then((res) => {
                    console.log(res);
                    if (res.data.JWTToken) {
                        localStorage.setItem("user", JSON.stringify(res.data));
                    }
                    Swal.close()
                    swal("Success!", "You have successfully logged in! You will be redirected to the homepage in 1 second.", "success");
                    // Redirect to homepage
                    setTimeout(() => {
                        this.props.history.push("/");
                    }, 500);
                })
                .catch((err) => {
                    Swal.close()
                    swal("Error!", "An error has occured. Incorrect username or email and password combination", "error");
                    throw err;
                })
            // Exit loading loop
            loading = false;
        }
    }


    render() {
        return (
            <form
                id="login"
                method='post'
                onSubmit={this.login}>
                <h3>Login</h3>

                <div className="form-group">
                    <label>Username or Email</label>
                    <input type="text" name="usernameoremail" className="form-control" placeholder="Enter your username or email" />
                </div>


                <div className="form-group">
                    <label>Password</label>
                    <input type="password" name="password" className="form-control" placeholder="Enter your password" />
                </div>

                <button type="submit" className="btn btn-dark btn-lg btn-block">Login</button>
            </form>
        );
    }
}

export default withRouter(LoginUserAccount);