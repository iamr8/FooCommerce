import './../styles/components/_navbar-bottom.scss';
import React, { Component } from 'react';
import Nav from 'react-bootstrap/Nav';

import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { library } from '@fortawesome/fontawesome-svg-core'
import { fas } from '@fortawesome/free-solid-svg-icons'
import { faHome, faHeart, faPuzzlePiece, faCircleUser } from '@fortawesome/free-solid-svg-icons'

library.add(fas, faHome, faHeart, faPuzzlePiece, faCircleUser)

interface IState {
	collapsed: boolean;
}

export class NavMenuBottom extends Component<{}, IState> {
	static displayName = NavMenuBottom.name;

	constructor(props: any) {
		super(props);

		this.toggleNavbar = this.toggleNavbar.bind(this);
		this.state = {
			collapsed: true
		};
	}

	toggleNavbar() {
		this.setState({
			collapsed: !this.state.collapsed
		});
	}

	render() {
		return (
			<nav className="navbar navbar-bottom">
				<ul className="navbar-nav">
					<Nav.Item>
						<Nav.Link href="#/" active={true}>
							<span className="nav-link-image">
								<FontAwesomeIcon icon={["fas", "home"]} />
							</span>
							<span className="nav-link-name">
								Home
							</span>
						</Nav.Link>
					</Nav.Item>
					<Nav.Item>
						<Nav.Link href="#/">
							<span className="nav-link-image">
								<FontAwesomeIcon icon={["fas", "puzzle-piece"]} />
							</span>
							<span className="nav-link-name">
								Categories
							</span>
						</Nav.Link>
					</Nav.Item>
					<Nav.Item>
						<Nav.Link href="/counter">
							<span className="nav-link-image">
								<FontAwesomeIcon icon={["fas", "heart"]} />
							</span>
							<span className="nav-link-name">
								Bookmarks
							</span>
						</Nav.Link>
					</Nav.Item>
					<Nav.Item>
						<Nav.Link href="/fetch-data">
							<span className="nav-link-image">
								<FontAwesomeIcon icon={["fas", "circle-user"]} />
							</span>
							<span className="nav-link-name">
								Menu
							</span>
						</Nav.Link>
					</Nav.Item>
				</ul>
			</nav>
		);
	}
}
