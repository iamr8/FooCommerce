import { library } from '@fortawesome/fontawesome-svg-core';
import { faBriefcase, faCar, faCouch, faHand, faHome, faPersonDress, fas, faStore, faTelevision, faWrench } from '@fortawesome/free-solid-svg-icons';
import { faFacebook, faInstagram, faTwitch, faTwitter, faYoutube } from "@fortawesome/free-brands-svg-icons"
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import "../styles/components/btn-icon.scss";
import "../styles/home.scss";
import ListingCategories from './services/Categories';

library.add(fas, faHome, faCar, faStore, faCouch, faHand, faTelevision, faWrench, faBriefcase, faPersonDress, faFacebook, faInstagram, faTwitter, faYoutube);
export class Home extends Component {
	static displayName = Home.name;

	render() {
		return <div className="home-page">
			<h2 className="home-pad home-welcome-message">Welcome to Tablo</h2>
			<div className="container home-categories">
				<div className="row">
					<div className="col-12 text-center mb-3">
						Please select the appropriate category to get started.
					</div>
				</div>
				<div className="row">
					{ListingCategories.map((route, index) => {
						const { title, icon, ...rest } = route;
						return <div className="col-4">
							<Link key={index} {...rest} className="btn btn-icon">
								<div className='btn-icon-container'>
									<FontAwesomeIcon icon={['fas', icon]} />
								</div>
								<span className="btn-text">
									{title}
								</span>
							</Link>
						</div>
					})}
				</div>
			</div>
			<h2 className="mt-5 mb-4">Buy and Sell DIRECTLY!</h2>
			<div className="container">
				<div className="row justify-content-center">
					<div className="col-8">
						<a href="/place-ad" className="btn btn-primary">
							Place an Ad
						</a>
					</div>
				</div>
			</div>
			<div className="container mt-auto mb-3 social-media-container">
				<div className="row">
					<div className="col-12 text-center">
						Follow us on social media to get the latest updates.
					</div>
				</div>
				<div className="row mt-3">
					<div className="col-3">
						<a href="https://www.facebook.com/tabllo" className="btn btn-icon social-media">
							<FontAwesomeIcon icon={faFacebook} />
						</a>
					</div>
					<div className="col-3">
						<a href="https://www.facebook.com/tabllo" className="btn btn-icon social-media">
							<FontAwesomeIcon icon={faInstagram} />
						</a>
					</div>
					<div className="col-3">
						<a href="https://www.facebook.com/tabllo" className="btn btn-icon social-media">
							<FontAwesomeIcon icon={faTwitter} />
						</a>
					</div>
					<div className="col-3">
						<a href="https://www.facebook.com/tabllo" className="btn btn-icon social-media">
							<FontAwesomeIcon icon={faYoutube} />
						</a>
					</div>
				</div>
			</div>
		</div>
	}
}
