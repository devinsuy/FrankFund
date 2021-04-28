import React, { useState } from 'react';
import Link from '@material-ui/core/Link';
import { makeStyles } from '@material-ui/core/styles';
import Typography from '@material-ui/core/Typography';
//import Title from './Title';


const useStyles = makeStyles({
    depositContext: {
        flex: 1,
    },
});

export default function Savings() {
    const classes = useStyles();
    const year = new Date().getFullYear()
    const [dataFetched, setDataFetched] = useState(false);
    const [savingsAmt, setSavingsAmt] = useState(null)

    async function fetchData(){
        let user = JSON.parse(localStorage.getItem("user"));
        let apikey = "c55f8d138f6ccfd43612b15c98706943e1f4bea3";
        let url = `/api/Analytics/TotalSavings/ThisYear&user=${user.AccountUsername}&apikey=${apikey}`;

        await(
            fetch(url)
            .then((data) => data.json())
            .then((savingsData) => {
                setSavingsAmt(savingsData.TotalSavings);
            })
        )
        .catch((err) => { 
            console.log(err) 
        });        
        setDataFetched(true);
    }

    if(!dataFetched){
        fetchData();
    }
    

    return ( !dataFetched ? <><Typography component="h2" variant="h6" color="primary" gutterBottom>Recent Savings</Typography> <a>Loading . . .</a></>  :
        <React.Fragment>
            <Typography component="h2" variant="h6" color="primary" gutterBottom>
                Recent Savings
            </Typography>
            <Typography component="p" variant="h4" id="animateValue">
                { "$" + savingsAmt.toLocaleString()}
            </Typography>
                    <Typography color="textSecondary" className={classes.depositContext}>
                        since Jan 1st, {year}
            </Typography>
            <div>
                <Link color="primary" href="/goals">
                    View goals
                </Link>
            </div>
        </React.Fragment>
    );
}