import React from 'react';
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

    return (
        <React.Fragment>
            <Typography component="h2" variant="h6" color="primary" gutterBottom>
                Recent Savings
            </Typography>
            <Typography component="p" variant="h4">
                $3,024.00
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