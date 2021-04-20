// JavaScript source code for User Account Login React Component

import React, { Component } from "react";
import axios from 'axios';
import swal from 'sweetalert';

export default class LoginUserAccount extends Component {

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
                swal("Success!", "You have successfully logged in! You will be redirected to the homepage in 1 second.", "success");
                setTimeout(() => {
                    this.$router.push("/");
                }, 1000);
            })
            .catch((err) => {
                swal("Error!", "An error has occured. Incorrect username or email and password combination", "error");
                throw err;
            });
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
                    <input type="text" name="UsernameEmail" className="form-control" placeholder="Enter your username or email" />
                </div>


                <div className="form-group">
                    <label>Password</label>
                    <input type="password" name="Password" className="form-control" placeholder="Enter your password" />
                </div>

                <button type="submit" className="btn btn-dark btn-lg btn-block">Login</button>
            </form>
        );
    }
}