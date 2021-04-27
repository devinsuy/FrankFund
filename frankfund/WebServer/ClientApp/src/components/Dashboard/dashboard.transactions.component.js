/* global varName */
/* eslint-env jquery */
import Swal from 'sweetalert2'

export default function Transactions({transactions}) {
    return (
        <>
            { transactions.map((transaction) => (<Transaction key={transaction.TID} transaction={transaction} />))}
        </>
    )
}

const Transaction = ({ transaction }) => {
    // Convert date to readable format 
    let dateMade = transaction.DateTransactionMade != "" ? new Date(transaction.DateTransactionMade.replace(/-/g, '\/')).toDateString() : "";
    let dateEntered = transaction.DateTransactionEntered != "" ? new Date(transaction.DateTransactionEntered.replace(/-/g, '\/')).toDateString() : "";

    // Build api url for this particular goal
    let apikey = "c55f8d138f6ccfd43612b15c98706943e1f4bea3";
    let url = `/api/Transaction/TID=${transaction.TID}&apikey=${apikey}`;

    return (
        <>
            <tr id={`Transaction${transaction.TID}`} key={transaction.TID}>
                <td id={`TransactionName${transaction.TID}`}> {transaction.TransactionName}</td>
                <td id={`Amount${transaction.TID}`}> {transaction.Amount != "" ? "$" + transaction.Amount : ""}</td>
                <td id={`DateTransactionMade${transaction.TID}`}> {dateMade}</td>
                <td id={`DateTransactionEntered${transaction.TID}`}> {dateEntered}</td>
                <td id={`IsExpense${transaction.TID}`}> {transaction.IsExpense == true ? "Expense" : transaction.IsExpense == false ? "Income" : ""}</td>
                <td id={`TransactionCategory${transaction.TID}`}> {transaction.TransactionCategory}</td>
            </tr>
        </>
    )
}