import Swal from 'sweetalert2'

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
    // Convert date to readable format 
    let endDate  = goal.EndDate != "" ? new Date(goal.EndDate).toDateString() : "";

    // Build api url for this particular goal
    let apikey = "c55f8d138f6ccfd43612b15c98706943e1f4bea3";
    let url = `/api/SavingsGoal/SGID=${goal.SGID}&apikey=${apikey}`;

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

    function editGoalAmount(){
        const inputValue = parseFloat(goal.GoalAmt)
        const inputStep = 0.01
        
        Swal.fire({
          title: goal.Name,
          showCancelButton: true,
          showCloseButton: true,
          html: `<p>Enter a new goal amount for <b>${goal.Name}</b>:</p>` +
            `<input type="number" value="${inputValue}" step="${inputStep}" class="swal2-input" id="range-value">`,
          input: 'range',
          inputValue,
          inputAttributes: {
            min: 1,
            max: parseFloat(goal.GoalAmt) * 5,
            step: inputStep
          },
          didOpen: () => {
            const inputRange = Swal.getInput()
            const inputNumber = Swal.getContent().querySelector('#range-value')
        
            // remove default output
            inputRange.nextElementSibling.style.display = 'none'
            inputRange.style.width = '100%'
        
            // sync input[type=number] with input[type=range]
            inputRange.addEventListener('input', () => {
              inputNumber.value = inputRange.value
            })
        
            // sync input[type=range] with input[type=number]
            inputNumber.addEventListener('change', () => {
              inputRange.value = inputNumber.value
            })
          }
        })
    }

    function editContribution(){
        const inputValue = parseFloat(goal.GoalAmt)
        const inputStep = 0.01
        
        Swal.fire({
          title: goal.Name,
          showCancelButton: true,
          showCloseButton: true,
          html: `<p>Enter a new contribution amount for <b>${goal.Name}</b>:</p>` +
            `<input type="number" value="${inputValue}" step="${inputStep}" class="swal2-input" id="range-value">`,
          input: 'range',
          inputValue,
          inputAttributes: {
            min: 1,
            max: parseFloat(goal.GoalAmt) * 5,
            step: inputStep
          },
          didOpen: () => {
            const inputRange = Swal.getInput()
            const inputNumber = Swal.getContent().querySelector('#range-value')
        
            // remove default output
            inputRange.nextElementSibling.style.display = 'none'
            inputRange.style.width = '100%'
        
            // sync input[type=number] with input[type=range]
            inputRange.addEventListener('input', () => {
              inputNumber.value = inputRange.value
            })
        
            // sync input[type=range] with input[type=number]
            inputNumber.addEventListener('change', () => {
              inputRange.value = inputNumber.value
            })
          }
        })
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
                            goal = goalData;
                            
                            // Update component without full re-render
                            document.getElementById(`Period${goal.SGID}`).innerHTML = goal.Period;
                            document.getElementById(`EndDate${goal.SGID}`).innerHTML = new Date(goal.EndDate).toDateString();

                            // Display success message
                            Swal.fire({
                                title: goal.Name,
                                icon: "success",
                                html: 
                                    `<p>Goal contribution period successfully updated from <b>${oldPeriod}</b> to <b>${goal.Period}</b>.</p>`
                                    + `<p>${goal.Name} end date was updated from <b>${endDate}</b> to <b>${goal.EndDate}</b>!</p>`,
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
        Swal.close();
    }


    




}


