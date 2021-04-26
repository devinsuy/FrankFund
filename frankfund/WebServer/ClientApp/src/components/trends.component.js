import React, { Component, useState, useEffect } from 'react';
import PropTypes from 'prop-types';

const propTypes = {
    type: PropTypes.bool,
};
  
const defaultProps = {
    type: undefined,
};

export default class Trends extends Component {
  constructor(props) {
    super(props);

    this.state = {
      foo: "bar"
    };
  }
  
  // const [stateItem, setStateItem] = useState(initialState);

  componentDidMount() {
  }
  
  // useEffect(() => {
  //   update.item = `{stateItem} to update`;
  // });


  render() {
    const { desctructureThis } = this; 
    const { destructureProps } = this.props;
    const { destructureState } = this.state;

    return (
        <div className="container">
            <h1 class="display-4 font-weight-bold white-text pt-5 mb-2">Trends</h1>
            <p>TODO</p>
        </div>
    );
  }
}

Trends.propTypes = propTypes;
Trends.defaultProps = defaultProps;
