// JavaScript source code for User Account Settings React Component

import React, { Component } from "react";
import axios from 'axios';
import swal from 'sweetalert';
import Swal from 'sweetalert2'

export default class SettingsUserAccount extends Component {

    constructor(props) {
        super(props);

        this.update = this.update.bind(this);
        this.deleteAcc = this.deleteAcc.bind(this);

    }

    update(e) {
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
                title: 'Updating account...',
                allowOutsideClick: false,
                onBeforeOpen: () => { Swal.showLoading() },
                onAfterClose() {
                    Swal.hideLoading()
                }
            });
            // Calls axios function to post the JSON data for PATCH request at API endpoint
            axios({
                method: "patch",
                url: "/api/account/accID=5&apikey=bd0eecf7cf275751a421a6101272f559b0391fa0",
                data: json,
                headers: {
                    'accept': 'application/json',
                    'content-type': 'application/json'
                }
            })
                .then((res) => {
                    console.log(res);
                    Swal.close()
                    swal("Success!", "Account has successfully been updated!", "success");
                })
                .catch((err) => {
                    swal("Error!", "An error has occured.", "error");
                    throw err;
                })
            // Exit loading loop
            loading = false;
        }
    }

    // Delete will delete the Account from the database and then
    // will log out the user, then redirect to landing page.
    deleteAcc(e) {
        e.preventDefault();
        Swal.fire({
            title: 'Are you sure you want to delete your account?',
            showDenyButton: true,
            showCancelButton: true,
            confirmButtonText: `Yes`,
            denyButtonText: `No`,
            customClass: {
                cancelButton: 'order-1 right-gap',
                confirmButton: 'order-2',
                denyButton: 'order-3',
            }
        }).then((result) => {
            if (result.isConfirmed) {
                let loading = true;
                while (loading) {
                    Swal.fire({
                        title: 'Deleting account...',
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
                        url: "/api/account/accID=5&apikey=bd0eecf7cf275751a421a6101272f559b0391fa0",
                        //data: json,
                        //headers: {
                        //    'accept': 'application/json',
                        //    'content-type': 'application/json'
                        //}
                    })
                        .then((res) => {
                            console.log(res);
                            Swal.close()
                            swal("Success!", "Account has successfully been deleted!", "success");
                        })
                        .catch((err) => {
                            swal("Error!", "An error has occured.", "error");
                            throw err;
                        })
                    // Exit loading loop
                    loading = false;
                }
            } else if (result.isDenied) {
                Swal.fire('Account has not been deleted.', '', 'info')
            }
        })
    }

    render() {
        const user = JSON.parse(localStorage.getItem('user'));
        return (
            <form
                id="account-settings"
                method= 'patch'
                onSubmit={this.update}>
                <h3>Account Settings</h3>

                <div className="form-group">
                    <label>Username</label>
                    <input type="text" name="AccountUsername" className="form-control" placeholder={user.AccountUsername} />
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

                <center><button onClick={this.deleteAcc} className="btn btn-outline-danger btn-lg btn-block">Delete Account</button></center>

                <button type="submit" className="btn btn-dark btn-lg btn-block">Update</button>
            </form>
        );
    }

}