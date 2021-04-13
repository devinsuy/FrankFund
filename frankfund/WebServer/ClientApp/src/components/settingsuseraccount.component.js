// JavaScript source code for User Account Settings React Component

import React, { Component } from "react";
import axios from 'axios';

export default class SettingsUserAccount extends Component {

    onSubmit(e) {
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
            method: "patch",
            url: "/api/account/accID={accID}&apikey=bd0eecf7cf275751a421a6101272f559b0391fa0",
            data: json,
            headers: {
                'accept': 'application/json',
                'content-type': 'application/json'
            }
        })
            .then((res) => {
                console.log(res);
            })
            .catch((err) => {
                throw err;
            });
    }

    render() {
        return (
            <form
                id="account-settings"
                //action='api/account/create'
                method= 'patch'
                onSubmit={this.onSubmit}>
                <h3>Account Settings</h3>

                <div className="form-group">
                    <label>Username</label>
                    <input type="text" name="AccountUsername" className="form-control" placeholder="Need to fill in with previous username" />
                </div>

                <div className="form-group">
                    <label>Email</label>
                    <input type="email" name="EmailAddress" className="form-control" placeholder="Need to fill in with previous email" />
                </div>

                <div className="form-group">
                    <label>Password</label>
                    <input type="password" name="Password" className="form-control" placeholder="Enter new password" />
                </div>

                <div className="form-group">
                    <label>Confirm Password</label>
                    <input type="password" className="form-control" placeholder="Confirm new password" />
                </div>

                <ul>
                    <li>Disable Account and confirmation needed</li>
                </ul>

                <button type="submit" className="btn btn-dark btn-lg btn-block">Update</button>
            </form>
        );
    }
}