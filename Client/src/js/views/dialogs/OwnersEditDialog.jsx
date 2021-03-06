import React from 'react';

import { connect } from 'react-redux';

import { Form, FormGroup, HelpBlock, ControlLabel } from 'react-bootstrap';

import _ from 'lodash';

import CheckboxControl from '../../components/CheckboxControl.jsx';
import EditDialog from '../../components/EditDialog.jsx';
import FilterDropdown from '../../components/FilterDropdown.jsx';
import FormInputControl from '../../components/FormInputControl.jsx';

import { isBlank } from '../../utils/string';

var OwnersEditDialog = React.createClass({
  propTypes: {
    owner: React.PropTypes.object,
    owners: React.PropTypes.object,
    localAreas: React.PropTypes.object,
    onSave: React.PropTypes.func.isRequired,
    onClose: React.PropTypes.func.isRequired,
    show: React.PropTypes.bool,
  },

  getInitialState() {
    var owner = this.props.owner;
    return {
      organizationName: owner.organizationName || '',
      givenName: owner.givenName || '',
      surname: owner.surname || '',
      address1: owner.address1 || '',
      address2: owner.address2 || '',
      city: owner.city || '',
      province: owner.province || '',
      postalCode: owner.postalCode || '',
      localAreaId: owner.localArea.id || 0,
      isMaintenanceContractor: owner.isMaintenanceContractor || false,
      doingBusinessAs: owner.doingBusinessAs || '',
      registeredCompanyNumber: owner.registeredCompanyNumber || '',
      status: owner.status || '',

      address1Error: '',
      cityError: '',
      provinceError: '',
      postalCodeError: '',
      organizationNameError: '',
      localAreaError: '',
    };
  },

  componentDidMount() {
    this.input.focus();
  },

  updateState(state, callback) {
    this.setState(state, callback);
  },

  didChange() {
    var owner = this.props.owner;

    if (this.state.organizationName !== owner.organizationName) { return true; }
    if (this.state.givenName !== owner.givenName) { return true; }
    if (this.state.surname !== owner.surname) { return true; }
    if (this.state.address1 !== owner.address1) { return true; }
    if (this.state.address2 !== owner.address2) { return true; }
    if (this.state.city !== owner.city) { return true; }
    if (this.state.province !== owner.province) { return true; }
    if (this.state.postalCode !== owner.postalCode) { return true; }
    if (this.state.localAreaId !== owner.localArea.id) { return true; }
    if (this.state.doingBusinessAs !== owner.doingBusinessAs) { return true; }
    if (this.state.registeredCompanyNumber !== owner.registeredCompanyNumber) { return true; }
    if (this.state.isMaintenanceContractor !== owner.isMaintenanceContractor) { return true; }

    return false;
  },

  isValid() {
    this.setState({
      companyAddressError: '',
      organizationNameError: '',
      localAreaError: '',
    });

    var valid = true;
    var owner = this.props.owner;
    var orgName = this.state.organizationName;

    if (isBlank(orgName)) {
      this.setState({ organizationNameError: 'Company name is required' });
      valid = false;
    } else if (orgName !== owner.organizationName) {
      // Does the name already exist?
      var nameIgnoreCase = orgName.toLowerCase().trim();
      var otherOwners = _.reject(this.props.owners.data, { id: owner.id });
      var other = _.find(otherOwners, other => other.organizationName.toLowerCase().trim() === nameIgnoreCase);
      if (other) {
        this.setState({ organizationNameError: 'This company name already exists in the system' });
        valid = false;
      }
    }

    if (isBlank(this.state.address1)) {
      this.setState({ address1Error: 'Address line 1 is required' });
      valid = false;
    }

    if (isBlank(this.state.city)) {
      this.setState({ cityError: 'City is required' });
      valid = false;
    }

    if (isBlank(this.state.province)) {
      this.setState({ provinceError: 'Province is required' });
      valid = false;
    }

    if (isBlank(this.state.postalCode)) {
      this.setState({ postalCodeError: 'Postal code is required' });
      valid = false;
    }

    if (this.state.localAreaId === 0) {
      this.setState({ localAreaError: 'Local area is required' });
      valid = false;
    }

    return valid;
  },

  onSave() {
    this.props.onSave({ ...this.props.owner, ...{
      organizationName: this.state.organizationName,
      givenName: this.state.givenName,
      surname: this.state.surname,
      address1: this.state.address1,
      address2: this.state.address2,
      city: this.state.city,
      province: this.state.province,
      postalCode: this.state.postalCode,
      localArea: { id: this.state.localAreaId },
      isMaintenanceContractor: this.state.isMaintenanceContractor,
      doingBusinessAs: this.state.doingBusinessAs,
      registeredCompanyNumber: this.state.registeredCompanyNumber,
      status: this.state.status,
    }});
  },

  render() {
    var owner = this.props.owner;
    var localAreas = _.sortBy(this.props.localAreas, 'name');

    return <EditDialog id="owners-edit" show={ this.props.show } 
      onClose={ this.props.onClose } onSave={ this.onSave } didChange={ this.didChange } isValid={ this.isValid }
      title= {
        <strong>Owner</strong>
      }>
      <Form>
        <FormGroup controlId="ownerCode">
          <ControlLabel>Owner Code</ControlLabel>
          <h4>{ owner.ownerCode }</h4>
        </FormGroup>
        <FormGroup controlId="organizationName" validationState={ this.state.organizationNameError ? 'error' : null }>
          <ControlLabel>Company Name <sup>*</sup></ControlLabel>
          <FormInputControl type="text" value={ this.state.organizationName } updateState={ this.updateState } inputRef={ ref => { this.input = ref; }} />
          <HelpBlock>{ this.state.organizationNameError }</HelpBlock>
        </FormGroup>
        <FormGroup controlId="givenName">
          <ControlLabel>Owner First Name</ControlLabel>
          <FormInputControl type="text" value={ this.state.givenName } updateState={ this.updateState } />
        </FormGroup>
        <FormGroup controlId="surname">
          <ControlLabel>Owner Last Name</ControlLabel>
          <FormInputControl type="text" value={ this.state.surname } updateState={ this.updateState } />
        </FormGroup>
        <FormGroup controlId="address1" validationState={ this.state.address1Error ? 'error' : null }>
          <ControlLabel>Address Line 1 <sup>*</sup></ControlLabel>
          <FormInputControl type="text" value={ this.state.address1 } updateState={ this.updateState } />
          <HelpBlock>{ this.state.address1Error }</HelpBlock>
        </FormGroup>
        <FormGroup controlId="address2">
          <ControlLabel>Address Line 2</ControlLabel>
          <FormInputControl type="text" value={ this.state.address2 } updateState={ this.updateState } />
        </FormGroup>
        <FormGroup controlId="city" validationState={ this.state.cityError ? 'error' : null }>
          <ControlLabel>City <sup>*</sup></ControlLabel>
          <FormInputControl type="text" value={ this.state.city } updateState={ this.updateState } />
          <HelpBlock>{ this.state.cityError }</HelpBlock>
        </FormGroup>
        <FormGroup controlId="province" validationState={ this.state.provinceError ? 'error' : null }>
          <ControlLabel>Province <sup>*</sup></ControlLabel>
          <FormInputControl type="text" value={ this.state.province } updateState={ this.updateState } />
          <HelpBlock>{ this.state.provinceError }</HelpBlock>
        </FormGroup>
        <FormGroup controlId="postalCode" validationState={ this.state.postalCodeError ? 'error' : null }>
          <ControlLabel>Postal Code <sup>*</sup></ControlLabel>
          <FormInputControl type="text" value={ this.state.postalCode } updateState={ this.updateState } />
          <HelpBlock>{ this.state.postalCodeError }</HelpBlock>
        </FormGroup>
        <FormGroup controlId="localAreaId" validationState={ this.state.localAreaError ? 'error' : null }>
          <ControlLabel>Local Area <sup>*</sup></ControlLabel>
          <FilterDropdown id="localAreaId" items={ localAreas } selectedId={ this.state.localAreaId } updateState={ this.updateState } className="full-width" />
          <HelpBlock>{ this.state.localAreaError }</HelpBlock>
        </FormGroup>
        <FormGroup controlId="doingBusinessAs">
          <ControlLabel>Doing Business As</ControlLabel>
          <FormInputControl type="text" value={ this.state.doingBusinessAs } updateState={ this.updateState } />
        </FormGroup>
        <FormGroup controlId="registeredCompanyNumber">
          <ControlLabel>Registered BC Company Number</ControlLabel>
          <FormInputControl type="text" value={ this.state.registeredCompanyNumber } updateState={ this.updateState } />
        </FormGroup>
        <FormGroup controlId="isMaintenanceContractor">
          <CheckboxControl id="isMaintenanceContractor" checked={ this.state.isMaintenanceContractor } updateState={ this.updateState }>Maintenance Contractor</CheckboxControl>
        </FormGroup>
      </Form>
    </EditDialog>;
  },
});

function mapStateToProps(state) {
  return {
    owner: state.models.owner,
    owners: state.lookups.owners,
    localAreas: state.lookups.localAreas,
  };
}

export default connect(mapStateToProps)(OwnersEditDialog);
