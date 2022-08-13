import React from 'react';
import "./../../styles/components/Listings/listing-overview.scss";

interface IProps extends React.HTMLAttributes<HTMLElement> {
	children?: React.ReactNode;
	isUrgent?: boolean;
	isDiscount?: boolean;
	isHot?: boolean;
	isPopular?: boolean;
	subject: string,
	price: string,
	updated: Date,
	onClick?: () => void;
	picture: string;
}

export class ListingOverview extends React.Component<IProps, {}> {
	static displayName = ListingOverview.name;

	render() {
		var badge = null;
		if (this.props.isUrgent) {
			badge = <span className="badge bg-primary">Urgent</span>;
		}
		return (
			<a href="#" {...this.props.onClick} className="listing-overview">
				<div className='listing-body'>
					<div className='listing-title'>
						<h3>{this.props.subject}</h3>
					</div>
					<div className='listing-badge'>
						{badge}
					</div>
					<div className='listing-price'>
						<h3>{this.props.price}</h3>
					</div>
					<div className='listing-updated'>
						<h3>{this.props.updated.toLocaleDateString()}</h3>
					</div>
				</div>
				<div className='listing-picture'>
					<div className='listing-picture-inner'>
						<picture>
							<source srcSet={this.props.picture} media="(min-width: 768px)" />
							<img src={this.props.picture} alt={this.props.subject} />
						</picture>
					</div>
				</div>
			</a>
		);
	}
}