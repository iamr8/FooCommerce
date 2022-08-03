import { library } from '@fortawesome/fontawesome-svg-core';
import { faFilter, faMagnifyingGlass, fas } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import React, { Component } from 'react';
import { Layout } from './../../../components/Layout';
import "../../../styles/listings.scss";

library.add(fas, faMagnifyingGlass, faFilter)

export class RealEstates extends Component<{}, {}> {
	static displayName = RealEstates.name;

	constructor(props: {}) {
		super(props);
	}

	render() {
		return (
			<Layout>
				<div className="input-group search-bar">
					<span className="input-group-text">
						<span className="btn">
							<FontAwesomeIcon icon={faMagnifyingGlass} />
						</span>
					</span>
					<input type="text" className="form-control" placeholder='Search in Listings' />
					<span className="input-group-text search-bar-button-container">
						<button className="btn btn-outline-secondary search-bar-button" type="button" title="Filters">
							<FontAwesomeIcon icon={faFilter} />
						</button>
					</span>
				</div>
				<div className="btn-toolbar search-toolbar" role="toolbar">
					<div className="btn-group" role="group" aria-label="Basic radio toggle button group">
						<input type="radio" className="btn-check" name="btnradio" id="btnradio1" autoComplete="off" checked />
						<label className="btn btn-outline-primary" htmlFor="btnradio1">Show All</label>

						<input type="radio" className="btn-check" name="btnradio" id="btnradio2" autoComplete="off" />
						<label className="btn btn-outline-primary" htmlFor="btnradio2">Urgent</label>

						<input type="radio" className="btn-check" name="btnradio" id="btnradio3" autoComplete="off" />
						<label className="btn btn-outline-primary" htmlFor="btnradio3">Discount</label>

						<input type="radio" className="btn-check" name="btnradio" id="btnradio4" autoComplete="off" />
						<label className="btn btn-outline-primary" htmlFor="btnradio4">Hot</label>

						<input type="radio" className="btn-check" name="btnradio" id="btnradio5" autoComplete="off" />
						<label className="btn btn-outline-primary" htmlFor="btnradio5">Popular</label>
					</div>
				</div>
			</Layout>
		)
	}
}