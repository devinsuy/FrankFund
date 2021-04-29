import React, { useState, useEffect } from 'react';
import { useTheme } from '@material-ui/core/styles';
import { Bar } from 'react-chartjs-2';
import Typography from '@material-ui/core/Typography';
import Link from '@material-ui/core/Link';
import Paper from '@material-ui/core/Paper';


export default function TopCategoryBars() {
    const [dataFetched, setDataFetched] = useState(false);
    const [data, setData] = useState({
        labels: ["Categories"],
        datasets: [
          {
            label: null,
            data: [],
            backgroundColor: [
              'rgba(255, 99, 132, 0.2)',
            ],
            borderColor: [
              'rgba(255, 99, 132, 1)',
              'rgba(54, 162, 235, 1)',
              'rgba(75, 192, 192, 1)',
            ],
            borderWidth: 1,
          },
          {
            label: null,
            data: [],
            backgroundColor: [
              'rgba(54, 162, 235, 0.2)',
            ],
            borderColor: [
              'rgba(54, 162, 235, 1)',
            ],
            borderWidth: 1,
          },
          {
            label: null,
            data: [],
            backgroundColor: [
              'rgba(75, 192, 192, 0.2)',
            ],
            borderColor: [
              'rgba(75, 192, 192, 1)',
            ],
            borderWidth: 1,
          },
        ],
      });

    async function fetchData(){
        let user = JSON.parse(localStorage.getItem("user"));
        let apikey = "c55f8d138f6ccfd43612b15c98706943e1f4bea3";
        let url = `/api/analytics/TopCategoryPercentages/AllTime&user=${user.AccountUsername}&apikey=${apikey}`;

        await(
            fetch(url)
            .then((data) => data.json())
            .then((dataList) => {
                let currData = data;
                let i = 0;
                dataList.CategoryPcts.forEach(function(item){
                    currData.datasets[i].data.push(item.pct)
                    currData.datasets[i].label = item.category;
                    i++;
                })
                setData(currData);
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


    const options= {
        scales: {
          y: {
            title: {
              display: true,
              text: "% of Total Spending"
            }
          }
        }
      }
    
    return (
        <>
        <Paper>
            <div className='header' align="center">
                <br></br>
                <Typography component="h2" variant="h6" color="primary" gutterBottom>Top Spending Categories</Typography>
            </div>
            {!dataFetched ? <> <a>Loading . . .</a></> :
            data.datasets[0].label == null ? <> <a> Nothing to display</a> </> :
                <Bar data={data} options={options} />
            }
        </Paper>

      </>
    );
}