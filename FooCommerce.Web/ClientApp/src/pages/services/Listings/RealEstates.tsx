import { library } from '@fortawesome/fontawesome-svg-core';
import { faArrowTrendUp, faFilter, faFire, faLayerGroup, faMagnifyingGlass, faPercent, fas, faStopwatch } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import React, { Component } from 'react';
import { Layout } from './../../../components/Layout';
import "../../../styles/listings.scss";
import { ListingOverview } from '../../../components/Listings/ListingOverview';

library.add(fas, faMagnifyingGlass, faFilter, faPercent, faStopwatch, faFire, faArrowTrendUp, faLayerGroup)

export class RealEstates extends Component<{}, {}> {
	static displayName = RealEstates.name;

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
						<label className="btn btn-outline-primary" htmlFor="btnradio1">
							<FontAwesomeIcon icon={faLayerGroup} />
							<span>All</span>
						</label>

						<input type="radio" className="btn-check" name="btnradio" id="btnradio2" autoComplete="off" />
						<label className="btn btn-outline-primary" htmlFor="btnradio2">
							<FontAwesomeIcon icon={faStopwatch} />
							<span>Urgent</span>
						</label>

						<input type="radio" className="btn-check" name="btnradio" id="btnradio3" autoComplete="off" />
						<label className="btn btn-outline-primary" htmlFor="btnradio3">
							<FontAwesomeIcon icon={faPercent} />
							<span>Discount</span>
						</label>

						<input type="radio" className="btn-check" name="btnradio" id="btnradio4" autoComplete="off" />
						<label className="btn btn-outline-primary" htmlFor="btnradio4">
							<FontAwesomeIcon icon={faFire} />
							<span>Hot</span>
						</label>

						<input type="radio" className="btn-check" name="btnradio" id="btnradio5" autoComplete="off" />
						<label className="btn btn-outline-primary" htmlFor="btnradio5">
							<FontAwesomeIcon icon={faArrowTrendUp} />
							<span>Popular</span>
						</label>
					</div>
				</div>
				<hr className="containerless" />
				<div className="listing-container">
					<ListingOverview isUrgent={true} price="30,000 TL" updated={new Date("2022/05/23")} picture="https://wp-tid.zillowstatic.com/25/ZG_BrandGTM_0321_GettyImages-528689860-RT-ed6165.jpg" subject="Urgent Sale"></ListingOverview>
					<ListingOverview isUrgent={true} price="30,000 TL" updated={new Date("2022/05/23")} picture="https://wp-tid.zillowstatic.com/25/ZG_BrandGTM_0321_GettyImages-528689860-RT-ed6165.jpg" subject="Urgent Sale"></ListingOverview>
					<ListingOverview isUrgent={true} price="30,000 TL" updated={new Date("2022/05/23")} picture="https://wp-tid.zillowstatic.com/25/ZG_BrandGTM_0321_GettyImages-528689860-RT-ed6165.jpg" subject="Urgent Sale"></ListingOverview>
					<ListingOverview isUrgent={true} price="30,000 TL" updated={new Date("2022/05/23")} picture="https://wp-tid.zillowstatic.com/25/ZG_BrandGTM_0321_GettyImages-528689860-RT-ed6165.jpg" subject="Urgent Sale"></ListingOverview>
					<ListingOverview isUrgent={true} price="30,000 TL" updated={new Date("2022/05/23")} picture="https://wp-tid.zillowstatic.com/25/ZG_BrandGTM_0321_GettyImages-528689860-RT-ed6165.jpg" subject="Urgent Sale"></ListingOverview>
				</div>
			</Layout>
		)
	}
}