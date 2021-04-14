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
    let endDate  = new Date(goal.EndDate).toDateString();

    // View button, display popup for additional information about goal 
    function view(){
        let startDate = new Date(goal.StartDate).toDateString();
        let noun = nouns[goal.Period];
        Swal.fire({
            title: goal.Name,
            text: "Hello",
            html:
            `<p>A savings goal for the amount of <b>$${goal.GoalAmt}</b> `
            + `will require an average <b>${goal.Period}</b> contribution of <b>$${goal.ContrAmt}</b> `
            + `for <b>${goal.NumPeriods}</b> ${noun} </p>`
            + `<p>${goal.Name} savings goal began on <b>${startDate}</b> and will end on <b>${endDate}</b></p>`
        })
    }
    
    // Edit button, display form to modify the goal
    function edit(){
        Swal.fire({
            title: goal.Name,
            text:"hello"
        })
    }


    return (
        <>
            <tr key={goal.SGID}>
                <td>{goal.Name}</td>
                <td>{goal.GoalAmt != "" ? "$" + goal.GoalAmt : ""}</td>
                <td>{goal.ContrAmt != "" ? "$" + goal.ContrAmt : ""}</td>
                <td>{goal.Period}</td>
                {/* <td>{goal.NumPeriods}</td>
                <td>{goal.StartDate}</td> */}
                <td>{endDate}</td>
                <td>
                    <button onClick={view} className="btn btn-outline-success btn-sm">View</button>
                    <button onclick={edit} className="btn btn-outline-success btn-sm">Edit</button>
                    <button className="btn btn-outline-success btn-sm">Delete</button>
                </td>
            </tr>
        </>
    )
}


