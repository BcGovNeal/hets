import React from 'react';

import { connect } from 'react-redux';

import { browserHistory } from 'react-router';

import { Well, Row, Col, Alert, Button, ButtonGroup, Glyphicon, Label, DropdownButton, MenuItem } from 'react-bootstrap';
import { Link } from 'react-router';

import _ from 'lodash';
import Promise from 'bluebird';

import ContactsEditDialog from './dialogs/ContactsEditDialog.jsx';
import DocumentsListDialog from './dialogs/DocumentsListDialog.jsx';
import EquipmentAddDialog from './dialogs/EquipmentAddDialog.jsx';
import OwnersEditDialog from './dialogs/OwnersEditDialog.jsx';
import OwnersPolicyEditDialog from './dialogs/OwnersPolicyEditDialog.jsx';
import NotesDialog from './dialogs/NotesDialog.jsx';
import ChangeStatusDialog from './dialogs/ChangeStatusDialog.jsx';

import * as Action from '../actionTypes';
import * as Api from '../api';
import * as Constant from '../constants';
import * as Log from '../history';
import store from '../store';

import ColDisplay from '../components/ColDisplay.jsx';
import DeleteButton from '../components/DeleteButton.jsx';
import EditButton from '../components/EditButton.jsx';
import History from '../components/History.jsx';
import SortTable from '../components/SortTable.jsx';
import Spinner from '../components/Spinner.jsx';

import { formatDateTime, today, toZuluTime } from '../utils/date';
import { concat } from '../utils/string';

/*

TODO:
* Print / Notes / Policy Proof Documents (attachments)

*/

var OwnersDetail = React.createClass({
  propTypes: {
    owner: React.PropTypes.object,
    equipment: React.PropTypes.object,
    contact: React.PropTypes.object,
    documents: React.PropTypes.object,
    params: React.PropTypes.object,
    uiContacts: React.PropTypes.object,
    uiEquipment: React.PropTypes.object,
    router: React.PropTypes.object,
    notes: React.PropTypes.object,
  },

  getInitialState() {
    return {
      loading: true,

      showEditDialog: false,
      showContactDialog: false,
      showPolicyDialog: false,
      showPolicyDocumentsDialog: false,
      showEquipmentDialog: false,
      showDocumentsDialog: false,
      showNotesDialog: false,
      showChangeStatusDialog: false,

      contact: {},

      status: '',

      // Contacts
      uiContacts : {
        sortField: this.props.uiContacts.sortField || 'name',
        sortDesc: this.props.uiContacts.sortDesc  === true,
      },

      // Equipment
      uiEquipment : {
        sortField: this.props.uiEquipment.sortField || 'equipmentCode',
        sortDesc: this.props.uiEquipment.sortDesc  === true,
      },
    };
  },

  componentDidMount() {
    this.fetch().then(() => {
      if (this.props.params.contactId) {
        this.openContact(this.props);
      }
    });
  },

  componentWillReceiveProps(nextProps) {
    if (!_.isEqual(nextProps.params, this.props.params)) {
      if (nextProps.params.contactId &&  nextProps.params.contactId !== this.props.params.contactId) {
        this.openContact(nextProps);
      }
    }
  },

  openContact(props) {
    var contact = null;

    if (props.params.contactId === '0') {
      // New
      contact = {
        id: 0,
        owner: props.owner,
      };
    } else if (props.params.contactId) {
      // Open the contact for viewing if possible
      contact = props.owner.contacts[props.params.contactId];
    }

    if (contact) {
      this.openContactDialog(contact);
    } else {
      // Reset owner location
      this.props.router.push({
        pathname: this.props.owner.path,
      });
    }
  },

  fetch() {
    this.setState({ loading: true });

    var ownerId = this.props.params.ownerId;
    var ownerPromise = Api.getOwner(ownerId);
    var documentsPromise = Api.getOwnerDocuments(ownerId);
    var ownerNotesPromise = Api.getOwnerNotes(ownerId);

    return Promise.all([ownerPromise, documentsPromise, ownerNotesPromise]).finally(() => {
      this.setState({ loading: false });
    });
  },

  updateContactsUIState(state, callback) {
    this.setState({ uiContacts: { ...this.state.uiContacts, ...state }}, () => {
      store.dispatch({ type: Action.UPDATE_OWNER_CONTACTS_UI, ownerContacts: this.state.uiContacts });
      if (callback) { callback(); }
    });
  },

  updateEquipmentUIState(state, callback) {
    this.setState({ uiEquipment: { ...this.state.uiEquipment, ...state }}, () => {
      store.dispatch({ type: Action.UPDATE_OWNER_EQUIPMENT_UI, ownerEquipment: this.state.uiEquipment });
      if (callback) { callback(); }
    });
  },

  showNotes() {

  },

  updateStatusState(state) {
    if (state !== this.props.owner.status) {
      Log.ownerModifiedStatus(this.props.owner, state);
      this.setState({ status: state }, this.openChangeStatusDialog());
    }
  },

  openChangeStatusDialog() {
    this.setState({ showChangeStatusDialog: true });
  },

  closeChangeStatusDialog() {
    this.setState({ showChangeStatusDialog: false });
  },

  onChangeStatus(status) {
    Api.changeOwnerStatus(status).then(() => {
      this.closeChangeStatusDialog();
    });
  },

  showDocuments() {
    this.setState({ showDocumentsDialog: true });
  },

  closeDocumentsDialog() {
    this.setState({ showDocumentsDialog: false });
  },

  addNote() {

  },

  addDocument() {

  },

  openEditDialog() {
    this.setState({ showEditDialog: true });
  },

  closeEditDialog() {
    this.setState({ showEditDialog: false });
  },

  saveEdit(owner) {
    // This just ensures that the normalized data doesn't mess up the PUT call
    Api.updateOwner({ ...owner, contacts: null }).finally(() => {
      Log.ownerModified(this.props.owner);
      this.closeEditDialog();
    });
  },

  openContactDialog(contact) {
    this.setState({
      contact: contact,
      showContactDialog: true,
    });
  },

  closeContactDialog() {
    this.setState({ showContactDialog: false }, () => {
      // Reset owner location
      this.props.router.push({
        pathname: this.props.owner.path,
      });
    });
  },

  addContact() {
    this.props.router.push({
      pathname: `${ this.props.owner.path }/${ Constant.CONTACTS_PATHNAME }/0`,
    });
  },

  deleteContact(contact) {
    Api.deleteContact(contact).then(() => {
      Log.ownerContactDeleted(this.props.owner, this.props.contact).then(() => {
        // In addition to refreshing the contacts, we need to update the owner
        // to get primary contact info and history.
        this.fetch();
      });
    });
  },

  saveContact(contact) {
    var isNew = !contact.id;
    var log = isNew ? Log.ownerContactAdded : Log.ownerContactUpdated;

    Api.addOwnerContact(this.props.owner, contact).then(() => {
      // Use this.props.contact to get the contact id
      return log(this.props.owner, this.props.contact);
    }).finally(() => {
      // In addition to refreshing the contacts, we need to update the owner
      // to get primary contact info and history.
      this.fetch();
      this.closeContactDialog();
    });
  },

  openEquipmentDialog() {
    this.setState({ showEquipmentDialog: true });
  },

  closeEquipmentDialog() {
    this.setState({ showEquipmentDialog: false });
  },

  saveNewEquipment(equipment) {
    Api.addEquipment(equipment).then(() => {
      // Open it up
      Log.ownerEquipmentAdded(this.props.owner, this.props.equipment);
      Log.equipmentAdded(this.props.equipment);
      this.props.router.push({
        pathname: `${Constant.EQUIPMENT_PATHNAME}/${this.props.equipment.id}`,
        state: { returnUrl: `${Constant.OWNERS_PATHNAME}/${this.props.owner.id}` },
      });
    });
  },

  equipmentVerifyAll() {
    var now = today();
    var owner = this.props.owner;

    // Update the last verified date on all pieces of equipment
    var equipmentList =_.map(owner.equipmentList, equipment => {
      return {...equipment, ...{
        lastVerifiedDate: toZuluTime(now),
        owner: { id: owner.id },
      }};
    });

    Api.updateOwnerEquipment(owner, equipmentList);
  },

  equipmentVerify(equipment) {
    Api.updateEquipment({...equipment, ...{
      lastVerifiedDate: toZuluTime(today()),
      owner: { id: this.props.owner.id },
    }}).then(() => {
      Log.ownerEquipmentVerified(this.props.owner, equipment);
      this.fetch();
    });
  },

  openPolicyDialog() {
    this.setState({ showPolicyDialog: true });
  },

  closePolicyDialog() {
    this.setState({ showPolicyDialog: false });
  },

  savePolicyEdit(owner) {
    // This just ensures that the normalized data doesn't mess up the PUT call
    Api.updateOwner({ ...owner, contacts: null }).finally(() => {
      Log.ownerModifiedPolicy(this.props.owner);
      this.closePolicyDialog();
    });
  },

  openPolicyDocumentsDialog() {
    // TODO Show popup with links to policy documents
    this.setState({ showPolicyDocumentsDialog: true });
  },

  closePolicyDocumentsDialog() {
    this.setState({ showPolicyDocumentsDialog: false });
  },

  addPolicyDocument() {
    // TODO Upload policy document (proof of policy coverage)
  },

  openNotesDialog() {
    this.setState({ showNotesDialog: true });
  },

  closeNotesDialog() {
    this.setState({ showNotesDialog: false });
  },

  print() {
    window.print();
  },


  getStatusDropdownStyle() {
    switch(this.props.owner.status) {
      case(Constant.OWNER_STATUS_CODE_APPROVED):
        return 'success';
      case(Constant.OWNER_STATUS_CODE_PENDING):
        return 'danger';
      default: 
        return 'default';
    }
  },

  render() {
    var owner = this.props.owner;
    
    return <div id="owners-detail">
      <div>
        {(() => {
          var dropdownItems = _.pull([ Constant.OWNER_STATUS_CODE_APPROVED, Constant.OWNER_STATUS_CODE_PENDING, Constant.OWNER_STATUS_CODE_ARCHIVED ], owner.status);

          if (this.state.loading) { return <div className="spinner-container"><Spinner/></div>; }

          return <Row id="owners-top">
            <Col md={8}>
              {/* <Label bsStyle={ owner.isApproved ? 'success' : 'danger'}>{ owner.status }</Label> */}
              <DropdownButton
                bsStyle={ this.getStatusDropdownStyle() }
                title={ owner.status }
                onSelect={ this.updateStatusState }
              >
              { _.map(dropdownItems.map((item, i) =>
                <MenuItem key={ i } eventKey={ item }>{ item }</MenuItem>
              ))}
              </DropdownButton>
              <Label className={ owner.isMaintenanceContractor ? 'ml-5' : 'hide' }>Maintenance Contractor</Label>
              <Button className="ml-5 mr-5" title="Notes" onClick={ this.openNotesDialog }>Notes ({ Object.keys(this.props.notes).length })</Button>
              <Button title="Documents" onClick={ this.showDocuments }>Documents ({ Object.keys(this.props.documents).length })</Button>
            </Col>
            <Col md={4}>
              <div className="pull-right">
                {/* <DropdownControl id="status" title={ owner.status } updateState={ this.updateStatusState } staticTitle={true}
                  items={_.pull([ Constant.OWNER_STATUS_CODE_APPROVED, Constant.OWNER_STATUS_CODE_PENDING, Constant.OWNER_STATUS_CODE_ARCHIVED ], owner.status)} /> */}
                <Button className="mr-5" onClick={ this.print }><Glyphicon glyph="print" title="Print" /></Button>
                <Button title="Return to List" onClick={ browserHistory.goBack }><Glyphicon glyph="arrow-left" /> Return to List</Button>
              </div>
            </Col>
          </Row>;
        })()}

        {(() => {
          return <div id="owners-header">
            <Row>
              <Col md={12}>
                <h1>Company: <small>{ owner.organizationName }</small></h1>
              </Col>
            </Row>
          </div>;
        })()}

        <Row>
          <Col md={12}>
            <Well>
              <h3>Owner Information <span className="pull-right">
                <Button title="Edit Owner" bsSize="small" onClick={ this.openEditDialog }><Glyphicon glyph="pencil" /></Button>
              </span></h3>
              {(() => {
                if (this.state.loading) { return <div className="spinner-container"><Spinner/></div>; }
                
                return <div id="owners-data">
                  <Row>
                    <ColDisplay md={6} labelProps={{ md: 6 }} label="Company">{ owner.organizationName }</ColDisplay>
                    <ColDisplay md={6} labelProps={{ md: 6 }} label="Company Address">{ owner.fullAddress }</ColDisplay>
                  </Row>
                  <Row>
                    <ColDisplay md={6} labelProps={{ md: 6 }} label="Owner Name">{ owner.ownerName }</ColDisplay>
                    <ColDisplay md={6} labelProps={{ md: 6 }} label="Owner Code">{ owner.ownerCode }</ColDisplay>
                  </Row>
                  <Row>
                    <ColDisplay md={6} labelProps={{ md: 6 }} label="Primary Contact">{ owner.primaryContactName }</ColDisplay>
                    <ColDisplay md={6} labelProps={{ md: 6 }} label="Doing Business As">{ owner.doingBusinessAs }</ColDisplay>
                  </Row>
                  <Row>
                    <ColDisplay md={6} labelProps={{ md: 6 }} label="Registered BC Company Number">{ owner.registeredCompanyNumber }</ColDisplay>
                    <ColDisplay md={6} labelProps={{ md: 6 }} label="District Office">{ owner.districtName }</ColDisplay>
                  </Row>
                  <Row>
                    <ColDisplay md={6} labelProps={{ md: 6 }} label="Service/Local Area">{ owner.localAreaName }</ColDisplay>
                    <ColDisplay md={6} labelProps={{ md: 6 }} label="Meets Residency?">{ owner.meetsResidency ? 'Yes' : 'No' }</ColDisplay>
                  </Row>
                </div>;
              })()}
            </Well>
          </Col>
          <Col md={12}>
            <Well>
              <h3>Policy <span className="pull-right">
                <Button title="Edit Policy Information" bsSize="small" onClick={ this.openPolicyDialog }><Glyphicon glyph="pencil" /></Button>
              </span></h3>
              {(() => {
                if (this.state.loading) { return <div className="spinner-container"><Spinner/></div>; }

                return <div id="owners-policy">
                  <Row>
                    <ColDisplay md={12} labelProps={{ md: 4 }} label="WorkSafeBC Policy">{ owner.workSafeBCPolicyNumber }</ColDisplay>
                  </Row>
                  <Row>
                    <ColDisplay md={12} labelProps={{ md: 4 }} label="WorkSafeBC Expiry Date">
                      { formatDateTime(owner.workSafeBCExpiryDate, Constant.DATE_YEAR_SHORT_MONTH_DAY) }
                    </ColDisplay>
                  </Row>
                  <Row>
                    <ColDisplay md={12} labelProps={{ md: 4 }} label="CGL Policy End Date">
                      { formatDateTime(owner.cglEndDate, Constant.DATE_YEAR_SHORT_MONTH_DAY) }
                    </ColDisplay>
                  </Row>
                </div>;
              })()}
            </Well>
          </Col>
          <Col md={12}>
            <Well>
              <h3>Contacts</h3>
              {(() => {
                if (this.state.loading ) { return <div className="spinner-container"><Spinner/></div>; }

                var addContactButton = <Button title="Add Contact" onClick={ this.addContact } bsSize="xsmall"><Glyphicon glyph="plus" />&nbsp;<strong>Add</strong></Button>;

                if (!owner.contacts || Object.keys(owner.contacts).length === 0) { return <Alert bsStyle="success">No contacts { addContactButton }</Alert>; }

                var contacts = _.sortBy(owner.contacts, this.state.uiContacts.sortField);
                if (this.state.uiContacts.sortDesc) {
                  _.reverse(contacts);
                }

                var headers = [
                  { field: 'name',         title: 'Name'  },
                  { field: 'phone',        title: 'Phone' },
                  { field: 'emailAddress', title: 'Email' },
                  { field: 'role',         title: 'Role'  },
                  { field: 'addContact',   title: 'Add Contact', style: { textAlign: 'right'  },
                    node: addContactButton,
                  },
                ];

                return <SortTable id="contact-list" sortField={ this.state.uiContacts.sortField } sortDesc={ this.state.uiContacts.sortDesc } onSort={ this.updateContactsUIState } headers={ headers }>
                  {
                    _.map(contacts, (contact) => {
                      return <tr key={ contact.id }>
                        <td>{ contact.isPrimary && <Glyphicon glyph="star" /> } { contact.name }</td>
                        <td>{ contact.phone }</td>
                        <td><a href={ `mailto:${ contact.emailAddress }` } target="_blank">{ contact.emailAddress }</a></td>
                        <td>{ contact.role }</td>
                        <td style={{ textAlign: 'right' }}>
                          <ButtonGroup>
                            <DeleteButton name="Contact" hide={ !contact.canDelete || contact.isPrimary } onConfirm={ this.deleteContact.bind(this, contact) }/>
                            <EditButton name="Contact" view={ !contact.canEdit } pathname={ contact.path }/>
                          </ButtonGroup>
                        </td>
                      </tr>;
                    })
                  }
                </SortTable>;
              })()}
            </Well>
            <Well>
              <h3>History</h3>
              { owner.historyEntity && <History historyEntity={ owner.historyEntity } refresh={ !this.state.loading } /> }
            </Well>
          </Col>
          <Col md={12}>
            <Well>
              <h3>Equipment ({ owner.numberOfEquipment }) <span className="pull-right">
                <Button className="mr-5" title="Verify All Equipment" bsSize="small" onClick={ this.equipmentVerifyAll }>Verify All</Button>
                <Button title="Add Equipment" bsSize="small" onClick={ this.openEquipmentDialog }><Glyphicon glyph="plus" /></Button>
              </span></h3>
              {(() => {
                if (this.state.loading) { return <div className="spinner-container"><Spinner/></div>; }

                if (!owner.equipmentList || owner.equipmentList.length === 0) { return <Alert bsStyle="success">No equipment</Alert>; }

                var equipmentList = _.sortBy(owner.equipmentList, this.state.uiEquipment.sortField);
                if (this.state.uiEquipment.sortDesc) {
                  _.reverse(equipmentList);
                }

                var headers = [
                  { field: 'equipmentCode',    title: 'ID'                  },
                  { field: 'typeName',         title: 'Type'                },
                  { field: 'make',             title: 'Make/Model/Size' },
                  { field: 'lastVerifiedDate', title: 'Last Verified'       },
                  { field: 'blank' },
                ];

                return <SortTable id="equipment-list" sortField={ this.state.uiEquipment.sortField } sortDesc={ this.state.uiEquipment.sortDesc } onSort={ this.updateEquipmentUIState } headers={ headers }>
                  {
                    _.map(equipmentList, (equipment) => {
                      const location = {
                        pathname: `${Constant.EQUIPMENT_PATHNAME}/${equipment.id}`,
                        state: { returnUrl: `${Constant.OWNERS_PATHNAME}/${owner.id}` },
                      };
                      return <tr key={ equipment.id }>
                        <td><Link to={ location }>{ equipment.equipmentCode }</Link></td>
                        <td>{ equipment.typeName }</td>
                        <td>{ concat(equipment.make, concat(equipment.model, equipment.size, '/'), '/') }</td>
                        <td>{ equipment.isApproved ? formatDateTime(equipment.lastVerifiedDate, Constant.DATE_YEAR_SHORT_MONTH_DAY) : 'Not Approved' }</td>
                        <td style={{ textAlign: 'right' }}>
                          <Button title="Verify Equipment" bsSize="xsmall" onClick={ this.equipmentVerify.bind(this, equipment) }><Glyphicon glyph="ok" /> OK</Button>
                        </td>
                      </tr>;
                    })
                  }
                </SortTable>;
              })()}
            </Well>
          </Col>
        </Row>
      </div>
      { this.state.showEquipmentDialog &&
        <EquipmentAddDialog show={ this.state.showEquipmentDialog } onSave={ this.saveNewEquipment } onClose={ this.closeEquipmentDialog } />
      }
      { this.state.showEditDialog &&
        <OwnersEditDialog show={ this.state.showEditDialog } onSave={ this.saveEdit } onClose={ this.closeEditDialog } />
      }
      { this.state.showPolicyDialog &&
        <OwnersPolicyEditDialog show={ this.state.showPolicyDialog } onSave={ this.savePolicyEdit } onClose={ this.closePolicyDialog } />
      }
      { this.state.showContactDialog &&
        <ContactsEditDialog 
          show={ this.state.showContactDialog } 
          contact={ this.state.contact } 
          onSave={ this.saveContact } 
          onClose={ this.closeContactDialog } 
          isFirstContact={!this.props.owner.contacts || Object.keys(this.props.owner.contacts).length === 0}
        />
      }
      { this.state.showDocumentsDialog &&
        <DocumentsListDialog 
          show={ owner && this.state.showDocumentsDialog } 
          parent={ owner } 
          onClose={ this.closeDocumentsDialog } 
        />
      }
       { this.state.showNotesDialog &&
        <NotesDialog 
          show={ this.state.showNotesDialog } 
          onSave={ Api.addOwnerNote } 
          id={ this.props.params.ownerId }
          getNotes={ Api.getOwnerNotes }
          onUpdate={ Api.updateNote }
          onClose={ this.closeNotesDialog } 
          notes={ this.props.notes }
        />
      } 
      { this.state.showChangeStatusDialog &&
        <ChangeStatusDialog
          show={ this.state.showChangeStatusDialog}
          onClose={ this.closeChangeStatusDialog }
          onSave={ this.onChangeStatus }
          status={ this.state.status }
          parent={ owner }
          owner
        />
      }
    </div>;
  },
});


function mapStateToProps(state) {
  return {
    owner: state.models.owner,
    notes: state.models.ownerNotes,
    equipment: state.models.equipment,
    equipmentAttachments: state.models.equipmentAttachments,
    contact: state.models.contact,
    documents: state.models.documents,
    uiContacts: state.ui.ownerContacts,
    uiEquipment: state.ui.ownerEquipment,
  };
}

export default connect(mapStateToProps)(OwnersDetail);
