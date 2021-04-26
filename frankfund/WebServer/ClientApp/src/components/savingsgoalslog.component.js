import React, {Component, useState, useEffect} from "react";
import { withRouter } from 'react-router-dom';
import Goals from './savingsgoals.component';
import Swal from 'sweetalert2'

class SavingsGoalsLog extends Component{
    constructor(){
        super()
        this.state = {
            user: "",
            userID: -1,
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
        let user = JSON.parse(localStorage.getItem("user"));
        let apikey = "c55f8d138f6ccfd43612b15c98706943e1f4bea3";
        let url = `/api/account/user=${user.AccountUsername}/SavingsGoals&apikey=${apikey}`;

        // Retrieve all SavingsGoals for the user
        await(
            fetch(url)
            .then((data) => data.json())
            .then((goalsData) => {
                this.setState({ user: user.AccountUsername, userID: user.AccountID, goals: goalsData.Goals, dataFetched: true })
            })
        )
        .catch((err) => { 
            console.log(err) 
            this.setState({ user: user.AccountUsername, userID: user.AccountID, goals: this.emptyGoal, dataFetched: true })
        });
    };

    // Update retrieved goals
    componentWillMount() {
        this.getGoals()
    }

    render(){
        // UI prompt for creating a goal by date
        async function createByDate(userID){
            let apikey = "bd0eecf7cf275751a421a6101272f559b0391fa0";
            let url = `/api/SavingsGoal&apikey=${apikey}`;

            const { value: formValues } = await Swal.fire({
                title: "Create a Goal By Date",
                html:
                    '<h5>Savings Goal Name</h5>' +
                    '<input required id="swal-input1" class="swal2-input" placeholder="Enter a name" style="font-size: 16pt; height: 40px; width:280px;">' +
                    
                    '<h5>Goal Amount</h5>' +
                    '<input required id="swal-input2" class="swal2-input" placeholder="$ Enter amount" type="number" style="height: 40px">' +

                    '<h5>End Date</h5>' +
                    '<input required id="swal-input3" class="swal2-input" placeholder="$ Enter amount" type="date" style="font-size: 16pt; height: 40px; width:280px;">' +

                    '<h5>Contribution Period</h5>'
                        + `<label for="periods"></label>
                            <select name="periods" class="swal2-input" id="swal-input4" style="height: 40px; width:280px;">
                                <option value="Daily">Daily</option>
                                <option value="Weekly">Weekly</option>
                                <option value="BiWeekly">BiWeekly</option>
                                <option value="Monthly">Monthly</option>
                                <option value="BiMonthly">BiMonthly</option>
                            </select><br></br>`,
    

                focusConfirm: false,
                showCancelButton: true,
                showCloseButton: true,
                preConfirm: () => {
                    return [
                        document.getElementById("swal-input1").value,
                        document.getElementById("swal-input2").value,
                        document.getElementById("swal-input3").value,
                        document.getElementById("swal-input4").value
                    ];
                }
            });    
            
            // Create goal if user entered all all fields
            if (formValues) {
                let loading = true;
                while (loading) {
                    Swal.fire({
                        title: 'Creating Savings Goal',
                        html: `<p>Adding new goal <b>${formValues[0]}</b> ...</p>`,
                        allowOutSideClick: false,
                        onBeforeOpen: () => { Swal.showLoading() }
                    });
                    let params = {
                        method: "POST",
                        headers: { "Content-type": "application/json" },
                        body: JSON.stringify({
                            "AccountID": userID,
                            "Name": formValues[0],
                            "GoalAmt": formValues[1],
                            "EndDate": formValues[2],
                            "Period": formValues[3]
                        })
                    }
                    await (fetch(url, params))
                        .then(response => {
                            if (response.ok) {
                                // Display success message
                                Swal.fire({
                                    title: formValues[0],
                                    icon: "success",
                                    html: `<p>Goal has successfully created! Refreshing ...</p>`,
                                    showCloseButton: true
                                })
                                // Wait 1.5 seconds before reloading the page
                                setTimeout(function() { window.location.reload(false) }, 1500);                       
                            }
                            else {
                                Swal.fire({
                                    title: formValues[0],
                                    icon: "error",
                                    html: `Something went wrong, failed to create goal.</p>`,
                                    showCloseButton: true
                                })
                            }
                        })
                    //Exit loading loop
                    loading = false;
                }
            }        
        }

        // UI prompt for creating a goal by contribution
        async function createByContribution(userID){
            let apikey = "bd0eecf7cf275751a421a6101272f559b0391fa0";
            let url = `/api/SavingsGoal&apikey=${apikey}`;

            const { value: formValues } = await Swal.fire({
                title: "Create a Goal By Contribution",
                html:
                    '<h5>Savings Goal Name</h5>' +
                    '<input required id="swal-input1" class="swal2-input" placeholder="Enter a name" style="font-size: 16pt; height: 40px; width:280px;">' +
                    
                    '<h5>Goal Amount</h5>' +
                    '<input required id="swal-input2" class="swal2-input" placeholder="$ Enter amount" type="number" style="height: 40px">' +
    
                    '<h5>Contribution Amount</h5>' +
                    '<input required id="swal-input3" class="swal2-input" placeholder="$ Enter amount" type="number" style="height: 40px">' +

                    '<h5>Contribution Period</h5>'
                        + `<label for="periods"></label>
                            <select name="periods" class="swal2-input" id="swal-input4" style="height: 40px; width:280px;">
                                <option value="Daily">Daily</option>
                                <option value="Weekly">Weekly</option>
                                <option value="BiWeekly">BiWeekly</option>
                                <option value="Monthly">Monthly</option>
                                <option value="BiMonthly">BiMonthly</option>
                            </select><br></br>`,
                focusConfirm: false,
                showCancelButton: true,
                showCloseButton: true,
                preConfirm: () => {
                    return [
                        document.getElementById("swal-input1").value,
                        document.getElementById("swal-input2").value,
                        document.getElementById("swal-input3").value,
                        document.getElementById("swal-input4").value
                    ];
                }
            });    
            
            // Create goal if user entered all all fields
            if (formValues) {
                let loading = true;
                while (loading) {
                    Swal.fire({
                        title: 'Creating Savings Goal',
                        html: `<p>Adding new goal <b>${formValues[0]}</b> ...</p>`,
                        allowOutSideClick: false,
                        onBeforeOpen: () => { Swal.showLoading() }
                    });
                    let params = {
                        method: "POST",
                        headers: { "Content-type": "application/json" },
                        body: JSON.stringify({
                            "AccountID": userID,
                            "Name": formValues[0],
                            "GoalAmt": formValues[1],
                            "ContrAmt": formValues[2],
                            "Period": formValues[3]
                        })
                    }
                    await (fetch(url, params))
                        .then(response => {
                            if (response.ok) {
                                // Display success message
                                Swal.fire({
                                    title: formValues[0],
                                    icon: "success",
                                    html: `<p>Goal has successfully created! Refreshing ...</p>`,
                                    showCloseButton: true
                                })
                                // Wait 1.5 seconds before reloading the page
                                setTimeout(function() { window.location.reload(false) }, 1500);                       
                            }
                            else {
                                Swal.fire({
                                    title: formValues[0],
                                    icon: "error",
                                    html: `Something went wrong, failed to create goal.</p>`,
                                    showCloseButton: true
                                })
                            }
                        })
                    //Exit loading loop
                    loading = false;
                }
            }
        }

        // Prompt radio selection UI for whether to create a goal by end date or contribution
        // returns boolean or null if user cancelled
        async function getByEndDate(){
            const inputOptions = new Promise((resolve) => {
                resolve({
                    'true' : "Goal by End Date",
                    'false' : "Goal by Contribution"
                })
            })
            const { value: byEndDate } = await Swal.fire({
                title: "Select Goal Type",
                input: 'radio',
                html: 
                `<p>Create a new goal by a <b>specified end date:</b></p> <p>Ex: Save $300 by December 25th</p> 
                <p>Create a new goal by <b>regular contribution amount:</b></p> <p>Ex: Save $300 by contributing $25 weekly</p><br></br>`,
                inputOptions: inputOptions,
                showCancelButton: true,
                showCloseButton: true,
                inputValidator: (value) => {
                    if (!value) {
                        return 'Please select an option'
                    }
                }
            })
            return byEndDate==null ? null : (byEndDate == 'true');
        }

        async function handleAddGoal(userID){
            // Prompt user for the goal type
            const isByDate = await(getByEndDate());
            
            if(isByDate == null) return;
            if(isByDate) createByDate(userID);
            else createByContribution(userID);
        }

        return (
            <div className="container">
                <h1 class="display-4 font-weight-bold white-text pt-5 mb-2">Savings Goals</h1>
                
                { // Pause loading if data has not been fetched yet
                !this.state.dataFetched ? <> <a>Loading . . .</a></> : 

                // Otherwise return loaded data
                <>
                    <button onClick={() => handleAddGoal(this.state.userID)} className="btn btn-dark btn-blk" style={{float: "right"}}>Add New Goal </button>
                    <h2>Hi {this.state.user}</h2>
                    <table className="table">
                        <thead>
                            <tr>
                                <th>Name</th>
                                <th>Goal Amount</th>
                                <th>Contribution Amount</th>
                                <th>Period</th>
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