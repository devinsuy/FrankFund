import React, { Component, useState, useEffect } from 'react';
import PropTypes from 'prop-types';
import CategoryChart from './category.polarchart.component';


export default function(){
    return (
        <div className="container">
            <h1 class="display-4 font-weight-bold white-text pt-5 mb-2">Trends</h1>
              <div align="left" style={{"width" : "50%", "size" : "50%"}}>
                <CategoryChart/>
              </div>
        </div>
    );
}
