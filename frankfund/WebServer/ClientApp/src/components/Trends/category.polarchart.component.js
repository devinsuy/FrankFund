import React, {Component, useState, useEffect} from "react";
import { PolarArea } from 'react-chartjs-2';
import Paper from '@material-ui/core/Paper';
import { makeStyles, withStyles } from '@material-ui/core/styles';
import Typography from '@material-ui/core/Typography';

const useStyles = makeStyles((theme) => ({
    paper: {
        padding: theme.spacing(2),
        display: 'flex',
        overflow: 'auto',
        flexDirection: 'column',
        height: 240
    },
}));

class CategoryChart extends Component {
    constructor(){
        super()
        this.state = {
            dataFetched : false,
            values: {
                datasets: [{
                    data: [0,0,0,0,0,0,0,0,0,0,0],
                    backgroundColor: [
                        "#FF6384", "#27AE60", "#4BC0C0", "#FFCE56", "#E7E9ED", "#36A2EB",
                        "#641E16", "#1B4F72", "#58D68D", "#EB984E", "#A569BD "
                    ],
                    label: 'Categorical Spending' // for legend
                }],
                labels: [
                    'Entertainment', 'Restaurants', 'Transportation', 'HomeAndUtilities', 'Education',
                    'Insurance', 'Health', 'Groceries', "Deposits", 'Shopping', 'Uncategorized'
                ]
            }
        };
    }

    async getCategoryData(){
        let user = JSON.parse(localStorage.getItem("user"));
        let apikey = "c55f8d138f6ccfd43612b15c98706943e1f4bea3";
        let url = `/api/Analytics/CategorySpending/AllTime&user=${user.AccountUsername}&apikey=${apikey}`;

        // Retrieve all SavingsGoals for the user
        await(
            fetch(url)
            .then((data) => data.json())
            .then((categoryData) => {
                let vals = []
                Object.keys(categoryData).forEach(function(key) {
                    vals.push(categoryData[key]);
                });
                console.log(vals);
                let currValues = this.state.values;
                currValues.datasets[0].data = vals;
                console.log(currValues)
                this.setState({values: currValues, dataFetched: true})
            })
        )
        .catch((err) => { 
            console.log(err) 
            this.setState({ values: this.state.values, dataFetched: true})
        });
    }

    componentDidMount(){
        this.getCategoryData();
    }
    
    render(){
        const { classes } = this.props;

        return( !this.state.dataFetched ? <> <a>Loading . . .</a> </> :
            <>
                <main class="container" ref={el => (this.div = el)}>
                    <Typography component="h2" variant="h6" color="primary" gutterBottom>
                        Categorical Breakdown
                    </Typography>
                    <Paper className={classes.paper}>
                        <PolarArea data={this.state.values}/>
                    </Paper>
                </main>
            </>
        )
    }
}

export default withStyles(useStyles)(CategoryChart)