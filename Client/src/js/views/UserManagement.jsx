import React from 'react';

import { connect } from 'react-redux';

import { PageHeader, Alert, Table, Button, Glyphicon } from 'react-bootstrap';

import _ from 'lodash';

import * as Api from '../api';

import Confirm from '../components/Confirm.jsx';
import OverlayTrigger from '../components/OverlayTrigger.jsx';
import Spinner from '../components/Spinner.jsx';


var UserManagement = React.createClass({
  propTypes: {
    currentUser : React.PropTypes.object,
    users : React.PropTypes.object,
    router : React.PropTypes.object,
  },

  getInitialState() {
    return {
      loading: false,
    };
  },

  componentDidMount() {
    this.fetch();
  },

  fetch(params) {
    this.setState({ loading: true });
    Api.getUsers(params).finally(() => {
      this.setState({ loading: false });
    });
  },

  edit(user) {
    this.props.router.push({
      pathname: 'user-management/' + user.id,
    });
  },

  delete(user) {
    console.debug(`delete ${user.name}!`);
  },

  render() {
    return <div id="user-management">
      <PageHeader>User Management</PageHeader>
      This feature has not been released yet.

      {(() => {
        if (this.state.loading) { return <div style={{ textAlign: 'center' }}><Spinner/></div>; }
        if (Object.keys(this.props.users).length === 0) { return <Alert bsStyle="info">No users</Alert>; }

        return <Table condensed striped>
          <thead>
            <tr>
              <th>Name</th>
              <th>Initials</th>
              <th></th>
            </tr>
          </thead>
          <tbody>
          {
            _.map(this.props.users, (user) => {
              return <tr key={ user.id } className={ !user.active ? 'info' : null }>
                <td style={{ verticalAlign: 'middle' }}><strong>{ user.surname }, { user.givenName }</strong></td>
                <td style={{ verticalAlign: 'middle' }}>{ user.initials }</td>
                <td style={{ textAlign: 'right' }}>
                  <Button onClick={ this.edit.bind(this, user) } style={{ marginRight: 10 }}><Glyphicon glyph="pencil" /> Edit</Button>
                  <OverlayTrigger trigger="click" placement="top" rootClose overlay={ <Confirm onConfirm={ this.delete.bind(this, user) }/> }>
                    <Button><Glyphicon glyph="trash" /> Delete</Button>
                  </OverlayTrigger>
                </td>
              </tr>;
            })
          }
          </tbody>
        </Table>;
      })()}

    </div>;
  },
});

function mapStateToProps(state) {
  return {
    currentUser : state.user,
    users       : state.models.users,
  };
}

export default connect(mapStateToProps)(UserManagement);
