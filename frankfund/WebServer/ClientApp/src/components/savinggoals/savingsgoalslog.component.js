import React, {Component, useState, useEffect} from "react";
import { withRouter } from 'react-router';
import Goals from '../savinggoals/savingsgoals.component';

class SavingsGoalsLog extends Component{
    constructor(){
        super()
        this.state = {
            user: "",
            goals: [],
            dataFetched : false
        };
        // Used only if User has no SavingsGoals
        this.emptyGoal = [{
            SGID: "", AccountID: "", Name: "No Goals To Display", GoalAmt: "", 
            ContrAmt: "", Period: "", SGID: "", StartDate: "", EndDate: "" 
        }]
    }

    async getGoals(){
        // TODO: Validation, if /goals/{user} != the logged in user return unauthorized (other users shouldn't be able to access data that isn't theirs)
        let user = this.props.match.params.user;                                // Select user from route /goals/{user}
        let apikey = "c55f8d138f6ccfd43612b15c98706943e1f4bea3";
        let url = `/api/account/user=${user}/SavingsGoals&apikey=${apikey}`;

        // Retrieve all SavingsGoals for the user
        await(
            fetch(url)
            .then((data) => data.json())
            .then((goalsData) => {
                this.setState({ user: user, goals: goalsData.Goals, dataFetched: true })
            })
        )
        .catch((err) => { 
            console.log(err) 
            this.setState({ user: user, goals: this.emptyGoal, dataFetched: true })
        });
    };

    // Update retrieved goals
    componentWillMount() {
        this.getGoals()
    }

    render(){
        return (
            <div className="container">
                <h1 class="display-4 font-weight-bold white-text pt-5 mb-2">Savings Goals</h1>
                
                { // Pause loading if data has not been fetched yet
                !this.state.dataFetched ? <> </> : 

                // Otherwise return loaded data
                <>
                    <b>Hi {this.state.user}</b>
                    <table className="table">
                        <thead>
                            <tr>
                                <th>Name</th>
                                <th>Goal Amount</th>
                                <th>Contribution Amount</th>
                                <th>Period</th>
                                {/* <th>Number of Periods</th>
                                <th>Start Date</th> */}
                                <th>End Date</th>
                                <th></th>
                            </tr>
                        </thead>
                        <tbody>
                            {<Goals goals={this.state.goals} />}
                            {this.state.dataFetched = false}
                        </tbody>
                    </table>
                </>
                }  
            </div>
        );
    }
}

export default withRouter(SavingsGoalsLog);