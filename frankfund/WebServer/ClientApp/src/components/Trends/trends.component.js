import React, { Component, useState, useEffect } from 'react';
import PropTypes from 'prop-types';
import CategoryChart from './category.polarchart.component';
import RatioChart from './spendratio.linechart.component';
import { makeStyles } from '@material-ui/core/styles';
import Grid from '@material-ui/core/Grid';
import Paper from '@material-ui/core/Paper';
import clsx from 'clsx';

export default function(){
    const useStyles = makeStyles((theme) => ({
      paper: {
          padding: theme.spacing(2),
          display: 'flex',
          overflow: 'auto',
          flexDirection: 'column',
      },
      fixedHeight: {
          height: 240,
      },
    }));
    const classes = useStyles();
    const fixedHeightPaper = clsx(classes.paper, classes.fixedHeight);

    return (
        <div className="container">
            <h1 class="display-4 font-weight-bold white-text pt-5 mb-2">Trends</h1> <hr></hr>
              <Grid item xs={12} md={8} lg={9}>
                <div align="left" style={{"width" : "50%", "size" : "65%"}}>
                  <CategoryChart/> <br></br>
                </div>
                <Paper className={fixedHeightPaper}>
                    <RatioChart />
                </Paper>
              </Grid>
        </div>
    );
}
