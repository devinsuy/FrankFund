// JavaScript source code for User Account Settings React Component

import React, { Component } from "react";

export default class SettingsUserAccount extends Component {

    render() {
        return (
            <form
                id="account-settings"
                //action='api/account/create'
                method= 'post'
                onSubmit={this.onSubmit}>
                <h3>Account Settings</h3>

                <div className="form-group">
                    <label>Username</label>
                    <input type="text" className="form-control" placeholder="Need to fill in with previous username" />
                </div>

                <div className="form-group">
                    <label>Email</label>
                    <input type="email" className="form-control" placeholder="Need to fill in with previous email" />
                </div>

                <div className="form-group">
                    <label>Password</label>
                    <input type="password" className="form-control" placeholder="Enter new password" />
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