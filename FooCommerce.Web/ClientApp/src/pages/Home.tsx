import { IconName, library } from '@fortawesome/fontawesome-svg-core';
import { faBriefcase, faCar, faCouch, faHand, faHome, faPersonDress, fas, faStore, faTelevision, faWrench } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import * as Popup from '../components/Popup';
import "../styles/components/btn-icon.scss";
import "../styles/home.scss";
import ListingCategories from './services/Categories';

library.add(fas, faHome, faCar, faStore, faCouch, faHand, faTelevision, faWrench, faBriefcase, faPersonDress);
export class Home extends Component {
	static displayName = Home.name;

	render() {
		return <div className="home-page">
			<h2 className="home-pad home-welcome-message">Welcome to Tablo</h2>
			<div className="container home-categories">
				<div className="row">
					{ListingCategories.map((route, index) => {
						const { title, icon, ...rest } = route;
						return <div className="col-4">
							<Link key={index} {...rest} className="btn btn-icon">
								<div className='btn-icon-container'>
									<FontAwesomeIcon icon={['fas', icon as IconName]} />
								</div>
								<span className="btn-text">
									{title}
								</span>
							</Link>
						</div>
					})}
				</div>
			</div>
			<h2 className="home-pad">Buy and Sell DIRECTLY!</h2>
			<button type="button" className="btn btn-secondary" onClick={e => Popup.Container.show("testModal")}>Show Popup</button>
			<Popup.Container title="Test" id="testModal">
				<Popup.Body>Testing</Popup.Body>
			</Popup.Container>
		</div>
	}
}
