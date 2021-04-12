import React, {Component} from "react";

export default class CreateTransaction extends Component {
    render() {
        return (
            <form
                id="create-transaction"
                >
                <h3>Create a New Transaction</h3>

                <div className="form-group">
                    <label>Transaction Name</label>
                    <input type="text" className="form-control" placeholder="Enter the name" />
                </div>

                <div className="form-group">
                    <label>Amount</label>
                    <input type="text" className="form-control" placeholder="Enter the amount" />
                </div>

                <div className="form-group">
                    <label>Transaction Type</label><br></br>
                    <select>
                        <option value="expense">Expense</option>
                        <option value="income">Income</option>
                    </select>
                </div>

                <div className="form-group">
                    <label>Transaction Type</label><br></br>
                    <select>
                        <option value="expense">Expense</option>
                        <option value="income">Income</option>
                    </select>
                </div>

                <div className="form-group">
                    <label>Transaction Type</label><br></br>
                    <input type="date" id="start" name="trip-start" />
                </div>

                

                <button type="submit" className="btn btn-dark btn-lg btn-block">Add</button>
                <a href="\transactions-log"><button className="btn btn-dark btn-sm btn-block">Return</button></a>
            </form>
        )
    }
}