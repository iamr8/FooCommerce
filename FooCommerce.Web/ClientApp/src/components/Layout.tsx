import React, { Component } from 'react';
import { NavMenu } from '../components/NavMenu';
import { NavMenuBottom } from '../components/NavMenuBottom';
import "./../styles/layout.scss";

export class Layout extends Component<any, {}> {
	static displayName = Layout.name;

	render() {
		return (
			<>
				<main className="container-fluid">
					{this.props.children}
				</main>
				<NavMenuBottom />
			</>
		);
	}
}
