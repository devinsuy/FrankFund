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

export default function TopTransaction() {
    const classes = useStyles();
    const year = new Date().getFullYear()
    const [dataFetched, setDataFetched] = useState(false);
    const [data, setData] = useState(null)

    async function fetchData(){
        let user = JSON.parse(localStorage.getItem("user"));
        let apikey = "c55f8d138f6ccfd43612b15c98706943e1f4bea3";
        let url = `/api/Analytics/TopTransaction/AllTime&accID=${user.AccountID}&apikey=${apikey}`;

        await(
            fetch(url)
            .then((data) => data.json())
            .then((transactionData) => {
                console.log(transactionData.Transaction);
                setData(transactionData.Transaction);
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
                Largest Transaction
            </Typography>
            <Typography component="p" variant="h4" id="animateValue">
                { "$" + data.Amount.toLocaleString()}
            </Typography>
                    <Typography color="textSecondary" className={classes.depositContext}>
                        {'"' + data.TransactionName + '"' + " made on " 
                        + new Date(data.DateTransactionMade.replace(/-/g, '\/')).toDateString().slice(0,3) + ", " 
                        + new Date(data.DateTransactionMade.replace(/-/g, '\/')).toDateString().slice(4)
                        }
            </Typography>
            <div>
                <Link color="primary" href="/transactions">
                    View transactions
                </Link>
            </div>
        </React.Fragment>
    );
}