import React, { Component } from 'react';
import Nav from 'react-bootstrap/Nav';
import Navbar from 'react-bootstrap/Navbar'
import Container from 'react-bootstrap/Container'
import Offcanvas from 'react-bootstrap/Offcanvas';

import { Link } from 'react-router-dom';
import './../styles/components/_navbar.scss';

interface IState {
	collapsed: boolean;
}

export class NavMenu extends Component<{}, IState> {
	static displayName = NavMenu.name;

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
		const expand = "sm";
		return (
			<header>
				<Navbar key={expand} bg="light" expand={expand} className="mb-3">
					<Container fluid>
						<Navbar.Brand href="#/">Project1</Navbar.Brand>
						<Navbar.Toggle onClick={this.toggleNavbar} className="mr-2" aria-controls={`offcanvasNavbar-expand-${expand}`} />
						<Navbar.Offcanvas
							id={`offcanvasNavbar-expand-${expand}`}
							aria-labelledby={`offcanvasNavbarLabel-expand-${expand}`}
							placement="end"
						>
							<Offcanvas.Header closeButton>
								<Offcanvas.Title id={`offcanvasNavbarLabel-expand-${expand}`}>
									Offcanvas
								</Offcanvas.Title>
							</Offcanvas.Header>
							<Offcanvas.Body>
								<Nav className="justify-content-end flex-grow-1 pe-3">
									<Nav.Item>
										<Nav.Link className="text-dark" href="/">Home</Nav.Link>
									</Nav.Item>
									<Nav.Item>
										<Nav.Link className="text-dark" href="/counter">Counter</Nav.Link>
									</Nav.Item>
									<Nav.Item>
										<Nav.Link className="text-dark" href="/fetch-data">Fetch data</Nav.Link>
									</Nav.Item>
								</Nav>
							</Offcanvas.Body>
						</Navbar.Offcanvas>
					</Container>
				</Navbar>
			</header>
		);
	}
}
