import React, { Component } from 'react';
import { NavMenu } from '../components/NavMenu';
import { NavMenuBottom } from '../components/NavMenuBottom';

export class Layout extends Component<any, {}> {
	static displayName = Layout.name;

	render() {
		return (
			<>
				<NavMenu />
				<main className="container-fluid">
					{this.props.children}
				</main>
				<NavMenuBottom />
			</>
		);
	}
}