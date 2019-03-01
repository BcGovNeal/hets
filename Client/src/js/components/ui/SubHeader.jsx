import React from 'react';
import classNames from 'classnames';
import { Glyphicon } from 'react-bootstrap';

import TooltipButton from '../TooltipButton.jsx';


var SubHeader = React.createClass({
  propTypes: {
    title: React.PropTypes.string,
    id: React.PropTypes.string,
    className: React.PropTypes.string,
    editButtonTitle: React.PropTypes.string,
    editButtonDisabled: React.PropTypes.bool,
    editButtonDisabledTooltip: React.PropTypes.node,
    editIcon: React.PropTypes.string,
    onEditClicked: React.PropTypes.func,
    children: React.PropTypes.node,
  },

  render() {
    const {
      title,
      id,
      className,
      editButtonTitle,
      editButtonDisabled,
      editButtonDisabledTooltip,
      editIcon,
      children,
      onEditClicked,
    } = this.props;

    var editButton = children;
    if (onEditClicked && !children) {
      editButton = (
        <TooltipButton
          title={editButtonTitle}
          disabled={editButtonDisabled}
          disabledTooltip={editButtonDisabledTooltip}
          bsSize="small"
          onClick={onEditClicked}>
          <Glyphicon glyph={editIcon} />
        </TooltipButton>
      );
    }

    return (
      <h3 id={id} className={classNames('clearfix', 'ui-subheader', className)}>
        {title}
        <span className="pull-right">
          {editButton}
        </span>
      </h3>
    );
  },
});


SubHeader.defaultProps = {
  editIcon: 'pencil',
};


export default SubHeader;
