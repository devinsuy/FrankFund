import React from "react";
import { Link } from "react-router-dom";

// reactstrap components

export default class LandingComponent extends React.Component {

    componentDidMount() {
        const backgroundTag = document.getElementsByClassName("inner")[0];
        backgroundTag.style.width = "80%";
    }

    render() {
        return (
            <div class="row">
                <div class="col">
                                  
                    <div class="mask rgba-black-strong">

                        <div class="container-fluid d-flex align-items-center justify-content-center h-100">

                            <div class="row d-flex justify-content-center text-center">

                                <div class="col-md-10">

                                    <h2 class="display-4 font-weight-bold white-text pt-5 mb-2">FrankFund</h2>

                                    <h4 class="white-text my-4">Create a free account now to start making better financial decisions today!</h4>

                                    <h6 class="white-text my-6">FrankFund has many features such as: <br /></h6>
                                    <h7 class="white-text my-7">
                                        <li>Storing your financial transactions and receipts</li>
                                        <li>Creating goals for saving your money</li>
                                        <li>Sending notifications for upcoming subscription charges</li>
                                    </h7><br />
                                    <a href="/create-user-account" class="btn btn-outline-success" role="button">Sign Up</a>
                                </div>

                            </div>

                        </div>

                    </div>
                </div>
                <div class="col">
                    <img class="logo" src="https://lh3.googleusercontent.com/yMIRhv9YgrNwhWYatcVfb8l_BdoYdhYOy7jTPW42BaShPMQqmTxTKXM8XlX9pLZxzBes4HN1HpHIk52wW5drkxvwrQJ4G5_Q6pcBnOn4L_9zILLM41b--QxoUSVbHHVQKpW4Mfks4Q=w2400"></img>
                </div>
            </div >
        )
    }
}