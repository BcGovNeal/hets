import React from 'react';
import { ControlLabel, InputGroup, Button, Glyphicon } from 'react-bootstrap';

import _ from 'lodash';
import Moment from 'moment';
import DateTime from 'react-datetime';

var DateControl = React.createClass({
  propTypes: {
    id: React.PropTypes.string.isRequired,
    date: React.PropTypes.string,
    format: React.PropTypes.string,
    className: React.PropTypes.string,
    label: React.PropTypes.string,
    onChange: React.PropTypes.func,
    updateState: React.PropTypes.func,
    placeholder: React.PropTypes.string,
    title: React.PropTypes.string,
    disabled: React.PropTypes.bool,
    isValidDate: React.PropTypes.func,
  },

  clicked() {
    if (!this.props.disabled) {
      this.input.focus();
    }
  },

  dateChanged(date) {
    // ignore invalid dates
    if (_.isString(date) || !date || !date.isValid()) {
      return;
    }
    
    var dateString = date.format(this.props.format || 'YYYY-MM-DD');
    this.notifyValueChanged(dateString);
  },
  
  dateBlurred(date) {
    // when focus leaves input, if date is invalid, reset value to empty string
    if (_.isString(date) || !date || !date.isValid()) {
      this.notifyValueChanged('');
    }
  },

  notifyValueChanged(dateString) {
    // On change listener
    if (this.props.onChange) {
      this.props.onChange(dateString, this.props.id);
    }

    // Update state
    if (this.props.updateState) {
      this.props.updateState({
        [this.props.id]: dateString,
      });
    }
  },

  render() {
    var date = Moment.utc(this.props.date);
    var format = this.props.format || 'YYYY-MM-DD';

    var placeholder = this.props.placeholder || 'yyyy-mm-dd';
    var disabled = this.props.disabled;

    return <div className={ `date-control ${this.props.className || ''}` } id={ this.props.id }>
      {(() => {
        // Inline label
        if (this.props.label) { return <ControlLabel>{ this.props.label }</ControlLabel>; }
      })()}
      <InputGroup>
        <DateTime value={ date } dateFormat={ format } timeFormat={ false } closeOnSelect={ true } onChange={ this.dateChanged } onBlur={ this.dateBlurred }
          inputProps={{ placeholder: placeholder, disabled: disabled, ref: input => { this.input = input; } }} isValidDate={ this.props.isValidDate }
        />
        <InputGroup.Button>
          <Button onClick={ this.clicked }><Glyphicon glyph="calendar" title={ this.props.title }/></Button>
        </InputGroup.Button>
      </InputGroup>
    </div>;
  },
});


export default DateControl;
