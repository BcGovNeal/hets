import React from 'react';

import { connect } from 'react-redux';

import { Link } from 'react-router';

import { PageHeader, Well, Row, Col, Form, FormGroup, Alert, Button } from 'react-bootstrap';
import _ from 'lodash';

import * as Action from '../actionTypes';
import * as Api from '../api';
import * as Constant from '../constants';
import store from '../store';

import Main from './Main.jsx';
import Spinner from '../components/Spinner.jsx';
import ColDisplay from '../components/ColDisplay.jsx';
import SortTable from '../components/SortTable.jsx';
import FormInputControl from '../components/FormInputControl.jsx';

var BusinessPortal = React.createClass({
  propTypes: {
    user: React.PropTypes.object,
    business: React.PropTypes.object,
    uiOwners: React.PropTypes.object,
  },

  getInitialState() {
    return {
      loading: false,
      validating: false,
      success: false,
      errors: {},

      // owners
      uiOwners : {
        sortField: this.props.uiOwners.sortField || 'organizationName',
        sortDesc: this.props.uiOwners.sortDesc  === true,
      },
    };
  },

  componentDidMount() {
    this.fetch();
  },

  fetch() {
    this.setState({ loading: true, success: false });

    return Api.getBusiness().finally(() => {
      if (!_.isEmpty(this.props.business)) {
        this.setState({ success: true });
      }
      this.setState({ loading: false });
    });
  },

  updateState(state, callback) {
    this.setState(state, callback);
  },

  updateOwnersUIState(state, callback) {
    this.setState({ uiOwners: { ...this.state.uiOwners, ...state }}, () => {
      store.dispatch({ type: Action.UPDATE_OWNERS_UI, owners: this.state.uiOwners });
      if (callback) { callback(); }
    });
  },

  validateOwner(e) {
    e.preventDefault();

    this.setState({ validating: true, errors: {} });

    Api.validateOwner(this.state.secretKey, this.state.postalCode).then(() => {
      // clear input fields
      this.inputPostalCode.value = '';
      this.inputSecretKey.value = '';
    }).catch((err) => {
      console.error(err);
      this.setState({ errors: { secretKey: err } });
    }).finally(() => {
      this.setState({ validating: false });
    });
  },

  render() {
    return <Main showNav={false}>
      <div id="business-portal">
        <PageHeader>Business Portal</PageHeader>
        { this.state.loading && <div className="spinner-container"><Spinner/></div> }
        { !this.state.loading && this.state.success && this.renderPage() }
        { !this.state.loading && !this.state.success && this.renderError() }
      </div>
    </Main>;
  },

  renderPage() {
    var business = this.props.business;
    const hasErrors = Object.keys(this.state.errors).length > 0;

    return <div>
      <Row>
        <Col md={12}>
          <Well id="business-info">
            <h3>Business Information</h3>
            {(() => {
              return <div>
                <Row>
                  <Col lg={6} md={6} sm={12} xs={12}>
                    <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="Legal Name">{ business.bceidLegalName }</ColDisplay>
                  </Col>
                  <Col lg={6} md={6} sm={12} xs={12}>
                    <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="Doing Business As">{ business.bceidDoingBusinessAs }</ColDisplay>
                  </Col>
                </Row>
              </div>;
            })()}
          </Well>
        </Col>
        <Col md={12}>
          <Well id="owners">
            <h3>HETS District Owners Associated With Your BCeID</h3>
            {(() => {
              if (_.isEmpty(this.props.business.owners)) { return <Alert bsStyle="success">No district owners associated</Alert>; }

              var owners = _.sortBy(this.props.business.owners, this.state.uiOwners.sortField);
              if (this.state.uiOwners.sortDesc) {
                _.reverse(owners);
              }

              var headers = [
                { field: 'organizationName',   title: 'Name'  },
                { field: 'primaryContactName', title: 'Primary Contact' },
                { field: 'districtName',       title: 'District' },
                { field: 'localAreaName',      title: 'Local Area'  },
              ];

              return <SortTable id="owner-list" sortField={ this.state.uiOwners.sortField } sortDesc={ this.state.uiOwners.sortDesc } onSort={ this.updateOwnersUIState } headers={ headers }>
                {
                  _.map(owners, (owner) => {
                    return <tr key={ owner.id }>
                      <td><Link to={ `${Constant.BUSINESS_DETAILS_PATHNAME }/${owner.id}` }> {owner.organizationName}</Link></td>
                      <td>{ owner.primaryContactNameBusiness }</td>
                      <td>{ owner.districtNameBusiness }</td>
                      <td>{ owner.localAreaNameBusiness }</td>
                    </tr>;
                  })
                }
              </SortTable>;
            })()}
          </Well>
        </Col>
        <Col md={12}>
          <Well id="associate-owner">
            <h3>Associate HETS District Owner</h3>
            <div id="overview">
              <Row>
                <img id="hets-logo" title="Hired Equipment Tracking System" alt="Hired Equipment Tracking System" src="images/gov/hets.jpg"/>
                <p>
                  The Hired Equipment Program is for owners/operators who have a dump truck, bulldozer, backhoe or other piece of equipment they want to hire out to the Ministry Transportation and Infrastructure for day labour and emergency projects.
                </p>
                <p>
                  The Hired Equipment Program distributes available work to local equipment owners. The program is based on seniority and is designed to deliver work to registered users fairly and efficiently through the development of local area call-out lists.  Details about the Hired Equipment Program can be found <a href="https://www2.gov.bc.ca/gov/content/industry/construction-industry/transportation-infrastructure/hired-equipment-program">here</a>.
                </p>
              </Row>
              <p>
                If you are NEW to the Hired Equipment Program, contact your <a href="https://www2.gov.bc.ca/gov/content/industry/construction-industry/transportation-infrastructure/hired-equipment-program/need-help">local district office</a> to register your company and equipment.
              </p>
              <p>
                If you are REGISTERED with the Hired Equipment Program and this is your first time to the site, enter your Secret Key and Postal Code to validate your account, then select your account.
              </p>
              <p>
                For RETURNING equipment owners, select your company above to view your account.
              </p>
            </div>
            <Form inline onSubmit={this.validateOwner}>
              <FormGroup controlId="secretKey" validationState={this.state.errors.secretKey ? 'error' : null}>
                <FormInputControl
                  type="text"
                  placeholder="Please enter your secret key here"
                  className="mr-5"
                  disabled={this.state.validating}
                  defaultValue={ this.state.secretKey }
                  updateState={ this.updateState }
                  inputRef={input => this.inputSecretKey = input} />
              </FormGroup>
              <FormGroup controlId="postalCode" validationState={this.state.errors.postalCode ? 'error' : null}>
                <FormInputControl
                  type="text"
                  placeholder="Postal code"
                  className="mr-5"
                  disabled={this.state.validating}
                  defaultValue={ this.state.postalCode }
                  updateState={ this.updateState }
                  inputRef={input => this.inputPostalCode = input} />
              </FormGroup>
              <Button type="submit" disabled={this.state.validating}>
                Validate {this.state.validating && <Spinner />}
              </Button>
            </Form>
            { hasErrors && <div className="validation-error">Secret key validation failed.</div> }
          </Well>
        </Col>
      </Row>
    </div>;
  },

  renderError() {
    return <h1><small>An error was encountered. You may not have permission to access this page.</small></h1>;
  },
});

function mapStateToProps(state) {
  return {
    user: state.user,
    business: state.models.business,
    uiOwners: state.ui.owners,
  };
}

export default connect(mapStateToProps)(BusinessPortal);
