const Goal = ({ goal }) => {
    return (
        <>
            <tr key={goal.SGID}>
                <td>{goal.Name}</td>
                <td>{goal.GoalAmt != "" ? "$" + goal.GoalAmt : ""}</td>
                <td>{goal.ContrAmt != "" ? "$" + goal.ContrAmt : ""}</td>
                <td>{goal.Period}</td>
                <td>{goal.NumPeriods}</td>
                <td>{goal.StartDate}</td>
                <td>{goal.EndDate}</td>
            </tr>
        </>
    )
}

export default function Goals({goals}) {
    return (
        <>
            { goals.map((goal) => ( <Goal key={goal.SGID} goal={goal} /> ))}
        </>
    )
}
