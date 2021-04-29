import React, { Component, useState, useEffect } from 'react';
import PropTypes from 'prop-types';
import { makeStyles } from '@material-ui/core/styles';
import Grid from '@material-ui/core/Grid';
import Paper from '@material-ui/core/Paper';
import clsx from 'clsx';

import CategoryChart from './category.polarchart.component';
import TopCategoryBars from './topcategories.bargraph.component'
import RatioChart from './spendratio.linechart.component';
import TopTransaction from './TopTransaction';

export default function(){
    const useStyles = makeStyles((theme) => ({
      paper: {
          padding: theme.spacing(2),
          display: 'flex',
          overflow: 'auto',
          flexDirection: 'column',
      },
      fixedHeight: {
          height: 300,
      },
      fixedSmallerHeight: {
        height: 261,
      }
    }));
    const classes = useStyles();
    const fixedHeightPaper = clsx(classes.paper, classes.fixedHeight);
    const fixedSmallerPaper = clsx(classes.paper, classes.fixedSmallerHeight);

    return (
        <div className="container">
            <h1 class="display-4 font-weight-bold white-text pt-5 mb-2">Trends</h1> <hr></hr>
              <div style={{"display": "flex"}}>
                  <div style={{"width" : "40%"}}>
                    <CategoryChart/> <br></br>
                  </div>
                  <div style={{"width" : "95%", "height" : "80%"}}>
                    <TopCategoryBars/> <br></br>
                  </div>
                  <div style={{"width" : "50%"}}>
                    <Paper className={fixedSmallerPaper}>
                        <TopTransaction />
                    </Paper>
                  </div>
              </div>
              <Grid item xs={12} md={12} lg={12}>
                  <Paper className={fixedHeightPaper}>
                      <RatioChart />
                  </Paper>
              </Grid>

        </div>
    );
}
