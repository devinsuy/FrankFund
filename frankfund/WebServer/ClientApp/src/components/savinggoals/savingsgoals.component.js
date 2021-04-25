import Swal from 'sweetalert2'
import swal from 'sweetalert'
import $ from 'jquery';

export default function Goals({goals}) {
    return (
        <>
            { goals.map((goal) => ( <Goal key={goal.SGID} goal={goal} /> ))}
        </>
    )
}

// Map contribution periods to their corresponding noun
var nouns = {
    'Daily'     : 'day(s)',
    'Weekly'    : 'week(s)',
    'BiWeekly'  : 'period(s)',
    'Monthly'   : 'month(s)',
    'BiMonthly' : 'period(s)'
};

const Goal = ({ goal }) => {
    // Build api url for this particular goal
    let apikey = "c55f8d138f6ccfd43612b15c98706943e1f4bea3";
    let url = `/api/SavingsGoal/SGID=${goal.SGID}&apikey=${apikey}`;
    let endDate  = goal.EndDate != "" ? new Date(goal.EndDate).toDateString() : "";
    return (
        <>
            <tr id={`Goal${goal.SGID}`} key={goal.SGID}>
                <td id={`Name${goal.SGID}`}> {goal.Name}</td>
                <td id={`GoalAmt${goal.SGID}`}> {goal.GoalAmt != "" ? "$" + goal.GoalAmt : ""}</td>
                <td id={`ContrAmt${goal.SGID}`}> {goal.ContrAmt != "" ? "$" + goal.ContrAmt : ""}</td>
                <td id={`Period${goal.SGID}`}> {goal.Period}</td>
                <td id={`EndDate${goal.SGID}`}> {endDate}</td>
                <td>
                    <button onClick={viewAlert} className="btn btn-outline-success btn-sm">View</button>
                    <button onClick={editAlert} className="btn btn-outline-success btn-sm">Edit</button>
                    <button onClick={deleteAlert} className="btn btn-outline-success btn-sm">Delete</button>
                </td>
            </tr>
        </>
    )


    // ------------------------------ Button functionality ------------------------------

    // View button, display popup for additional information about goal 
    function viewAlert(){
        let startDate = new Date(goal.StartDate).toDateString();
        let endDate  = goal.EndDate != "" ? new Date(goal.EndDate).toDateString() : "";
        let noun = nouns[goal.Period];
        Swal.fire({
            title: goal.Name,
            icon: 'info',
            showCloseButton: true,
            html:
                `<p>A savings goal for the amount of <b>$${goal.GoalAmt}</b> `
                + `will require an average <b>${goal.Period}</b> contribution of <b>$${goal.ContrAmt}</b> `
                + `for <b>${goal.NumPeriods}</b> ${noun} </p>`
                + `<p>${goal.Name} savings goal began on <b>${startDate}</b> and will end on <b>${endDate}</b></p>`
        })
    }

    // Edit button, display form to modify the goal
    function editAlert(){
        Swal.fire({
            title: goal.Name,
            icon: "question",
            showConfirmButton: false,
            showCloseButton: true,
            html:
                `<p>Select a goal attibute to modify</p>
                <div>
                <button class="btn btn-primary" id="EditName${goal.SGID}">Name</button>
                <button class="btn btn-primary" id="EditGoal${goal.SGID}">Goal Amount</button>
                <button class="btn btn-primary" id="EditContr${goal.SGID}">Contibution Amount</button>
                <button class="btn btn-primary" id="EditPeriod${goal.SGID}">Period</button>
                <button class="btn btn-primary" id="EditEndDate${goal.SGID}">End Date</button>
                </div>`
        })

        // --------------- Button event handlers ---------------
        document.getElementById(`EditName${goal.SGID}`).addEventListener("click", function(){
            editName()
        });
        document.getElementById(`EditGoal${goal.SGID}`).addEventListener("click", function(){
            editGoalAmount();
        });
        document.getElementById(`EditContr${goal.SGID}`).addEventListener("click", function(){
            editContribution()
        });
        document.getElementById(`EditPeriod${goal.SGID}`).addEventListener("click", function(){
            editPeriod()
        });
        document.getElementById(`EditEndDate${goal.SGID}`).addEventListener("click", function(){
            editEndDate()
        });
    }

    function deleteAlert(){
        Swal.fire({
            title: `${goal.Name}`,
            html: `<p>Are you sure you want to <b>delete</b> your <b>${goal.Name}</b> savings goal? You won't be able to revert this!</p>`,
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Confirm',
            showCloseButton: true
        })
        document.getElementsByClassName(`swal2-confirm swal2-styled`)[0].addEventListener("click", function(){
            deleteGoal()
        });
    }

    // ------------------------------ Button operations functionality ------------------------------

    async function deleteGoal(){
        let loading = true;
        while(loading){
            Swal.fire({
                title: 'Updating',
                html: `<p>Deleting <b>${goal.Name}</b> savings goal</p>`,
                allowOutsideClick: false,
                onBeforeOpen: () => { Swal.showLoading()}
            });
            let params = { method: "DELETE" }
            await(fetch(url, params))
            .then(response => {    
                if(response.ok){
                    // Avoid re-rendering entire GoalsLog componeent, simply delete element
                    // (Fetch will reload changes from API if page refreshed)
                    document.getElementById(`Goal${goal.SGID}`).remove();

                    // Display success message
                    Swal.fire({
                        title: goal.Name,
                        icon: "success",
                        html: `<p>Savings goal <b>${goal.Name}</b> was successfully deleted!</p>`,
                        showCloseButton: true
                    }) 
                }
                else{
                    Swal.fire({
                        title: goal.Name,
                        icon: "error",
                        html: `<p>Something went wrong, failed to delete ${goal.Name} savings goal</p>`,
                        showCloseButton: true
                    })
                }
            }) 
            // Exit loading loop
            loading = false;
        }
    }

    // --------------- Edit goal attribute handlers ---------------

    async function editName(){
        const { value: newName } = await Swal.fire({
            title: goal.Name,
            showCloseButton: true,
            icon: "question",
            input: 'text',
            html: '<p>Enter a new name for the savings goal</p>',
            inputPlaceholder: 'Enter a new name',
            showCancelButton: true,
            inputValidator: (value) => {
                if (!value) {
                    return 'Please enter a name'
                }
            }
        })
        // Make PATCH request and update the name
        if(newName){
            let loading = true;
            while(loading){
                // Show loading message
                Swal.fire({
                    title: 'Updating',
                    html: `<p>Updating name from <b>${goal.Name}</b> to <b>${newName}</b></p>`,
                    allowOutsideClick: false,
                    onBeforeOpen: () => { Swal.showLoading() }
                });
                // Async await is blocking operation
                let params = {
                    method: "PATCH",  
                    headers: { "Content-type": "application/json" },  
                    body: JSON.stringify({ "Name" : newName })
                }
                await(fetch(url, params))
                .then(response => {    
                    if(response.ok){
                        // Avoid re-rendering entire GoalsLog componeent, simply update field 
                        // (Fetch will reload changes from API if page refreshed)
                        document.getElementById(`Name${goal.SGID}`).innerHTML = newName;
    
                        // Display success message
                        Swal.fire({
                            title: newName,
                            icon: "success",
                            html: `<p>Goal name successfully updated from <b>${goal.Name}</b> to <b>${newName}</b>!</p>`,
                            showCloseButton: true
                        }) 
                        goal.Name = newName;
                    }
                    else{
                        Swal.fire({
                            title: goal.Name,
                            icon: "error",
                            html: `<p>Something went wrong, failed to update name.</p>`,
                            showCloseButton: true
                        })
                    }
                }) 
                // Exit loading loop
                loading = false;           
            }
        }
    }


    // Prompt radio selection UI for whether to extend end date or not
    // param: Whether or not the new goal amount is > previous goal amount
    // returns boolean or null if user cancelled
    async function getExtendDate(dateVerb, contrVerb){
        const inputOptions = new Promise((resolve) => {
            let dateOption = dateVerb + ' End Date';
            let contrOption = contrVerb + ' Contribution';
            setTimeout(() => {
              resolve({
                'true' : dateOption,
                'false' : contrOption
              })
            }, 1000)
        })
        const { value: extendEndDate } = await Swal.fire({
            title: goal.Name,
            input: 'radio',
            html: `<p>Select whether you would like to <b>${dateVerb.toLowerCase()} the end date</b> 
                   or <b>${contrVerb.toLowerCase()} the ${goal.Period} contribution amount</b></p>`,
            inputOptions: inputOptions,
            showCancelButton: true,
            showCloseButton: true,
            inputValidator: (value) => {
                if (!value) {
                    return 'Please select an option'
                }
            }
        })
        return extendEndDate==null ? null : (extendEndDate == 'true');
    }

    async function editGoalAmount(){
        // Prompt user to enter a new goal amount        
        const { value: newGoalAmt } = await Swal.fire({
            title: goal.Name,
            input: 'text',
            showCancelButton: true,
            showCloseButton: true,
            inputPlaceholder: '$ Enter an amount',
            html: `<p>Your ${goal.Name} goal amount is currently set to <b>$${goal.GoalAmt}</b>, enter a new goal amount below.</p>`,
            inputValidator: (value) => {
              if(!value) {
                return 'Please enter a goal amount'
              }
              if(isNaN(value)){
                  return 'Please enter a valid amount'
              }
            }
          })
        // User entered a new goal amount, update the goal
        if(newGoalAmt){
            // Display no change made message if the goal amount submitted is the same
            if(newGoalAmt == goal.GoalAmt){
                Swal.fire({
                    title: goal.Name,
                    icon: "warning",
                    html: `<p>${goal.Name} goal amount is <b>already set to $${newGoalAmt}</b>! No changes were made.</p>`,
                    showCloseButton: true
                })    
                return;  
            }

            // Prompt user to either update the regular contribution amount or end date to 
            // reflect the changes in goal amount, do nothing if user cancels
            let dateVerb, dateVerb2, contrVerb, contrVerb2;
            if(parseFloat(goal.GoalAmt) > parseFloat(newGoalAmt)){
                dateVerb = "Shorten";
                dateVerb2 = "shortening";
                contrVerb = "Reduce";
                contrVerb2 = "reducing";
            }
            else{
                dateVerb = "Extend";
                dateVerb2 = "extending";
                contrVerb = "Increase";
                contrVerb2 = "increasing";
            }
            const extendDate = await getExtendDate(dateVerb, contrVerb);
            if(extendDate == null) { return; }

            let loading = true;
            while(loading){
                let secondAttributeMsg = extendDate ? `${dateVerb2} goal end date` : `${contrVerb2} ${goal.Period.toLowerCase()} contribution amount`;
                // Show loading message
                Swal.fire({
                    title: 'Updating',
                    html: `<p>Updating <b>${goal.Name}</b> goal amount from <b>$${goal.GoalAmt}</b> to <b>$${newGoalAmt}</b> and <b>${secondAttributeMsg}</b></p>`,
                    allowOutsideClick: false,
                    onBeforeOpen: () => { Swal.showLoading() }
                });
                let params = {
                    method: "PATCH",  
                    headers: { "Content-type": "application/json" },  
                    body: JSON.stringify({ "GoalAmt" : newGoalAmt, "ExtendDate" : extendDate })
                }
                await(
                    fetch(url, params)
                    .then((response) => response.json())
                    .then((goalData) => {  
                        // Update displayed fields
                        document.getElementById(`GoalAmt${goal.SGID}`).innerHTML = "$" + newGoalAmt.toString();
                        
                        // Generate HTML to display either the changed end date or the changed contribution amount resulting from the goal change
                        let secondChange;
                        if(extendDate){
                            let endDate  = goal.EndDate != "" ? new Date(goal.EndDate).toDateString() : "";
                            let newEndDate = new Date(goalData.EndDate).toDateString()
                            document.getElementById(`EndDate${goal.SGID}`).innerHTML = newEndDate;
                            secondChange = `<p>The end date of the goal was adjusted from <b>${endDate}</b> to <b>${newEndDate}</b></p>`
                        }
                        else{
                            document.getElementById(`ContrAmt${goal.SGID}`).innerHTML = "$" + goalData.ContrAmt.toString();
                            secondChange = `<p>The regular contribution amount of the goal was adjusted `
                             + `from <b>$${goal.ContrAmt.toString()}</b> to <b>$${goalData.ContrAmt.toString()}</b></p>`
                        }
    
                        // Display success message
                        Swal.fire({
                            title: goal.GoalName,
                            icon: "success",
                            html: 
                                `<p><b>${goal.Name}</b> goal amount successfully updated from <b>$${goal.GoalAmt}</b> to 
                                 <b>$${newGoalAmt}</b>!</p>` + secondChange,
                            showCloseButton: true
                        }) 
                        goal = goalData;
                }))
                .catch((err) => {
                    Swal.fire({
                        title: goal.Name,
                        icon: "error",
                        html: `<p>Something went wrong, failed to update goal amount.</p>`,
                        showCloseButton: true
                    })
                }) 
                loading = false;
            }
        }
    }


    async function editContribution(){
        // Prompt user to enter a new contribution amount        
        const { value: newContrAmt } = await Swal.fire({
            title: goal.Name,
            input: 'text',
            showCancelButton: true,
            showCloseButton: true,
            inputPlaceholder: '$ Enter an amount',
            html: `<p>Your ${goal.Name} goal currently has a <b>${goal.Period}</b> contribution of <b>$${goal.ContrAmt}</b>, enter a new contribution amount below.</p>`,
            inputValidator: (value) => {
              if(!value) {
                return 'Please enter a contribution amount'
              }
              if(isNaN(value)){
                  return 'Please enter a valid amount'
              }
            }
          })
        // User entered a new contribution amount, update the goal
        if(newContrAmt){
            // Display no change made message if the goal amount submitted is the same
            if(newContrAmt == goal.ContrAmt){
                Swal.fire({
                    title: goal.Name,
                    icon: "warning",
                    html: `<p>${goal.Name} goal <b>${goal.Period}</b> contribution is <b>already set to $${newContrAmt}</b>! No changes were made.</p>`,
                    showCloseButton: true
                })    
                return;  
            }

            // Prompt user to either update the regular contribution amount or end date to 
            // reflect the changes in goal amount, do nothing if user cancels
            let dateVerb = parseFloat(goal.ContrAmt) > parseFloat(newContrAmt) ? "extending" : "shortening"; 
            let loading = true;
            while(loading){
                let secondAttributeMsg = `${dateVerb} goal end date`;
                // Show loading message
                Swal.fire({
                    title: 'Updating',
                    html: `<p>Updating <b>${goal.Name}</b> goal <b>${goal.Period}</b> contribution from <b>$${goal.ContrAmt}</b> to <b>$${newContrAmt}</b> and <b>${secondAttributeMsg}</b></p>`,
                    allowOutsideClick: false,
                    onBeforeOpen: () => { Swal.showLoading() }
                });
                let params = {
                    method: "PATCH",  
                    headers: { "Content-type": "application/json" },  
                    body: JSON.stringify({ "ContrAmt" : newContrAmt })
                }
                await(
                    fetch(url, params)
                    .then((response) => response.json())
                    .then((goalData) => {  
                        // Update displayed fields
                        document.getElementById(`ContrAmt${goal.SGID}`).innerHTML = "$" + newContrAmt.toString();
                        
                        // Generate HTML to display either the changed end date or the changed contribution amount resulting from the goal change
                        let endDate  = goal.EndDate != "" ? new Date(goal.EndDate).toDateString() : "";
                        let newEndDate = new Date(goalData.EndDate).toDateString()
                        document.getElementById(`EndDate${goal.SGID}`).innerHTML = newEndDate;
                        let secondChange = `<p>The end date of the goal was adjusted from <b>${endDate}</b> to <b>${newEndDate}</b></p>`
    
                        // Display success message
                        Swal.fire({
                            title: goal.GoalName,
                            icon: "success",
                            html: 
                                `<p><b>${goal.Name}</b> goal <b>${goal.Period}</b> contribution successfully updated from <b>$${goal.ContrAmt}</b> to 
                                 <b>$${newContrAmt}</b>!</p>` + secondChange,
                            showCloseButton: true
                        }) 
                        goal = goalData;
                }))
                .catch((err) => {
                    Swal.fire({
                        title: goal.Name,
                        icon: "error",
                        html: `<p>Something went wrong, failed to update goal amount.</p>`,
                        showCloseButton: true
                    })
                }) 
                loading = false;
            }
        }
    }


    async function editPeriod(){
        // Prompt user with dropdown for period selection
        const { value: newPeriod } = await Swal.fire({
            title: `${goal.Name}`,
            text: "Select the new contribution period",
            input: 'select',
            icon: "question",
            showCloseButton: true,
            inputOptions: {
                Daily       : "Daily",
                Weekly      : "Weekly",
                BiWeekly    : "BiWeekly",
                Monthly     : "Monthly",
                BiMonthly   : "BiMonthly"
            },
            inputPlaceholder: 'Select a period',
            showCancelButton: true,
            inputValidator: (value) => {
                if (!value) {
                    return 'Please select a period'
                }
                return new Promise((resolve) => { resolve() })
            }
        })
        // Process input only if user presses submit
        if (newPeriod) {
            // Display message if selected period is the same as the old one
            if(newPeriod == goal.Period){
                Swal.fire({
                    title: goal.Name,
                    icon: "warning",
                    html: `<p>Goal contribution period is <b>already set to ${newPeriod}</b>! No changes were made.</p>`,
                    showCloseButton: true
                })              
            }
            // Make PATCH request and update the period
            else{
                let loading = true;
                let failed = false;
                while(loading){
                    // Show loading message
                    Swal.fire({
                        title: 'Updating',
                        html: `<p>Updating contribution period from <b>${goal.Period}</b> to <b>${newPeriod}</b></p>`,
                        allowOutsideClick: false,
                        onBeforeOpen: () => { Swal.showLoading()}
                    });
                    // Async await is blocking operation
                    let params = {  
                        method: "PATCH",  
                        headers: { "Content-type": "application/json" },  
                        body: JSON.stringify({ "Period" : newPeriod })
                    }
                    await(
                        fetch(url, params)
                        .then((response) => response.json())
                        .then((goalData) => {
                            // Update pointer to the updated goal JSON from the server
                            let oldPeriod = goal.Period;
                            let prevEndDate = goal.EndDate != "" ? new Date(goal.EndDate).toDateString() : "";
                            goal = goalData;
                            let newEndDate = goal.EndDate != "" ? new Date(goal.EndDate).toDateString() : "";
                            
                            // Update component without full re-render
                            document.getElementById(`Period${goal.SGID}`).innerHTML = goal.Period;
                            document.getElementById(`EndDate${goal.SGID}`).innerHTML = newEndDate

                            // Display success message
                            Swal.fire({
                                title: goal.Name,
                                icon: "success",
                                html: 
                                    `<p>Goal contribution period successfully updated from <b>${oldPeriod}</b> to <b>${goal.Period}</b>.</p>`
                                    + `<p>${goal.Name} end date was updated from <b>${prevEndDate}</b> to <b>${newEndDate}</b>!</p>`,
                                showCloseButton: true
                            })
                        })
                    )
                    .catch((err) => {
                        console.log(err);
                        Swal.fire({
                            title: goal.Name,
                            icon: "error",
                            html: `<p>Something went wrong, failed to update period.</p>`,
                            showCloseButton: true
                        })
                    });
                    // Exit loading loop
                    loading = false;           
                }
            }
        }
    }

    async function editEndDate(){
        // Swal.fire({
        //     titel: 'Enter date',
        //     html: '<div id="datepicker"></div>',
        //     onOpen: function() {
        //         $('#datepicker').datepicker();
        //     },
        // })
        // await( 
        //     swal({
        //         title: 'Date picker',
        //         html: '<div id="datepicker"></div>',
        //         onOpen: function() {
        //             $('#datepicker').datepicker();
        //         },
        //         preConfirm: function() {
        //             return Promise.resolve($('#datepicker').datepicker('getDate'));
        //         }
        //     }).then(function(result) {
        //         swal({
        //         type: 'success',
        //         html: 'You entered: <strong>' + result + '</strong>'
        //         });
        //     })
        // );
    }


    




}


