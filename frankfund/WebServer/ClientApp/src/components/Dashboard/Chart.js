import React, { useState } from 'react';
import { useTheme } from '@material-ui/core/styles';
import { LineChart, Line, XAxis, YAxis, Label, ResponsiveContainer } from 'recharts';
import Typography from '@material-ui/core/Typography';
import Link from '@material-ui/core/Link';
//import Title from './Title';


export default function Chart() {
    const theme = useTheme();
    const [dataFetched, setDataFetched] = useState(false);
    const [data, setData] = useState(null);

    async function fetchData(){
        let user = JSON.parse(localStorage.getItem("user"));
        let apikey = "c55f8d138f6ccfd43612b15c98706943e1f4bea3";
        let url = `/api/Analytics/ExpenseRatio/PastYear&user=${user.AccountUsername}&apikey=${apikey}`;

        await(
            fetch(url)
            .then((data) => data.json())
            .then((spendingData) => {
                setData(spendingData.MonthVals);
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
    
    return (
    // Pause loading if data has not been fetched yet
            !dataFetched ? <><Typography component="h2" variant="h6" color="primary" gutterBottom>Your Spending</Typography> <a>Loading . . .</a> </>: 

            // Otherwise return loaded data
            <>
                <Typography component="h2" variant="h6" color="primary" gutterBottom>
                    Your Spending
                </Typography>
                <ResponsiveContainer>
                    <LineChart
                        data={data}
                        margin={{
                            top: 16,
                            right: 16,
                            bottom: 0,
                            left: 24,
                        }}
                    >
                        <XAxis dataKey="month" stroke={theme.palette.text.secondary} />
                        <YAxis stroke={theme.palette.text.secondary} type ="number">
                            <Label
                                angle={270}
                                position="left"
                                style={{ textAnchor: 'middle', fill: theme.palette.text.primary }}
                            >
                                Spending ($)
                </Label>
                        </YAxis>
                        <Line type="monotone" dataKey="amt" stroke={theme.palette.primary.main} dot={false} />
                    </LineChart>
                </ResponsiveContainer>

                <div>
                    <Link color="primary" href="/trends">
                        View trends
                    </Link>
                </div>
            </>
    );
}



const counters = document.querySelectorAll('.counter');
const speed = 200; // The lower the slower

counters.forEach(counter => {
    const updateCount = () => {
        const target = +counter.getAttribute('data-target');
        const count = +counter.innerText;

        // Lower inc to slow and higher to slow
        const inc = target / speed;

        // console.log(inc);
        // console.log(count);

        // Check if target is reached
        if (count < target) {
            // Add inc to count and output in counter
            counter.innerText = count + inc;
            // Call function every ms
            setTimeout(updateCount, 1);
        } else {
            counter.innerText = target;
        }
    };

    updateCount();
});