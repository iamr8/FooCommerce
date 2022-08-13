import { library } from '@fortawesome/fontawesome-svg-core';
import { faMagnifyingGlass, fas } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import React, { Component } from 'react';
import { Layout } from '../components/Layout';
import "../styles/listings.scss";

library.add(fas, faMagnifyingGlass)

export class Listings extends Component<{}, {}> {
	static displayName = Listings.name;

	render() {
		return (
			<Layout>
				<div className="search-bar">
					<FontAwesomeIcon icon={faMagnifyingGlass} />
					<input type="text" placeholder="Search in Listings" />
				</div>
			</Layout>
		)
	}
}