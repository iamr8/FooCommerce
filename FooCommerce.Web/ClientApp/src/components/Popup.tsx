import "../styles/components/_popup.scss";
import React, { Component, createContext, ReactElement, useCallback, useContext, useEffect, useState } from "react";
import ReactDOM from "react-dom";
import UAParser from "ua-parser-js";
import { Modal as BootstrapModal } from "bootstrap";
import _ from 'lodash';
import classnames from "classnames";

type PopupChildrenProps = (
	| React.ReactElement<Body>
	| React.ReactElement<Footer>
)[];

interface IPopupState {
	container: HTMLDivElement,
	modal: BootstrapModal
}

interface IPopupProps extends React.HTMLAttributes<HTMLElement> {
	title?: string,
	showClose?: boolean,
	children: PopupChildrenProps | ReactElement<Body> | ReactElement<Footer>,
}

interface IHeaderProps extends React.HTMLAttributes<HTMLElement> {
	title: string,
	showClose?: boolean,
}


interface IBodyProps extends React.HTMLAttributes<HTMLElement> {
}

interface IFooterProps extends React.HTMLAttributes<HTMLElement> {
	children?: any,
	showClose?: boolean,
	closeText?: string,
}

interface IFooterState {
	showClose: boolean,
	closeText: string,
}

type PopupContextType = {
	hasFooter: boolean;
	Modal: BootstrapModal;
	ContainerElement: HTMLDivElement
}
const PopupContext = createContext<PopupContextType | null>({ hasFooter: false, Modal: undefined as unknown as BootstrapModal, ContainerElement: undefined as unknown as HTMLDivElement });

export class Container extends Component<IPopupProps, IPopupState> {
	static displayName = Container.name;
	static contextType = PopupContext;
	context!: React.ContextType<typeof PopupContext>;

	private body: React.ReactElement<Body>;
	private footer: React.ReactElement<Footer>;

	private modalElementRef: (element: HTMLDivElement) => void;

	private id: string;
	private title: string;

	constructor(props: IPopupProps) {
		super(props);

		if (!this.props.id)
			throw new Error("Popup must have an id");

		this.state = {
			modal: undefined as unknown as BootstrapModal,
			container: undefined as unknown as HTMLDivElement,
		};

		this.body = _.isArray(props.children)
			? props.children.find(child => child.type === Body) as ReactElement<Body>
			: props.children.type === Body ?
				props.children as ReactElement<Body>
				: undefined as unknown as ReactElement<Footer> || null;
		this.footer = _.isArray(props.children)
			? props.children.find(child => child.type === Footer) as ReactElement<Footer>
			: props.children.type === Footer ?
				props.children as ReactElement<Footer>
				: undefined as unknown as ReactElement<Footer> || null;

		this.title = this.props.title || "";

		this.id = this.props.id;

		this.modalElementRef = element => { this.setState({ container: element }) };
	}

	static async show(id: string) {
		const modalEl = document.querySelector(".modal#" + id) as HTMLDivElement;
		if (modalEl) {
			const modal = BootstrapModal.getOrCreateInstance(modalEl);
			modal.show();
		}
	}

	async componentDidMount() {
		console.debug("Popup Mounted.");
	}

	async componentDidUpdate(prevProps: Readonly<IPopupProps>, prevState: Readonly<IPopupState>, snapshot?: any) {
		_.isEqual(prevState, this.state) || console.debug("Popup Updated.", this.state);

		if (this.state.container && !this.state.modal) {
			const modal = BootstrapModal.getOrCreateInstance(this.state.container);
			this.setState({ modal: modal });
		}

	}

	render() {
		return ReactDOM.createPortal(
			<React.StrictMode>
				<PopupContext.Provider value={{ hasFooter: this.footer !== null, Modal: this.state.modal, ContainerElement: this.state.container }}>
					<div className={classnames({ modal: true, fade: true }, this.props.className?.split(" "))} id={this.id} tabIndex={-1} ref={this.modalElementRef}>
						<div className="modal-dialog modal-dialog-centered modal-dialog-scrollable">
							<div className="modal-content">
								<Header title={this.title} showClose={this.props.showClose}></Header>
								{this.body}
								{this.footer}
							</div>
						</div>
					</div>
				</PopupContext.Provider>
			</React.StrictMode>
			, document.querySelector("body") as HTMLBodyElement)
	}

}

const Header: React.FC<IHeaderProps> = (props) => {
	const context = useContext(PopupContext);

	const modalContentElementRef = React.useRef<HTMLDivElement>();
	const modalHeaderElementRef = React.useRef<HTMLDivElement>();
	const transitionDuration = React.useRef<number>(400);

	const [contentOriginalHeight, _setContentOriginalHeight] = useState(0);
	const [touchYStart, _setTouchYStart] = useState(0);

	const setContentOriginalHeight = useCallback((newValue: number) => _setContentOriginalHeight((prev) => newValue), []);
	const setTouchYStart = useCallback((newValue: number) => _setTouchYStart((prev) => newValue), []);
	const modalHeaderElementRefHandler = (el: HTMLDivElement) => modalHeaderElementRef.current = el;
	const setContentElement = React.useCallback((element: HTMLDivElement) => {

		console.debug("Callback setContentElement", element);
		modalContentElementRef.current = element;
		if (context?.ContainerElement && modalContentElementRef.current) {

			context?.ContainerElement.addEventListener("shown.bs.modal", () => {
				if (modalContentElementRef.current) {
					if (modalContentElementRef.current.clientHeight <= 0)
						return;

					setContentOriginalHeight(modalContentElementRef.current.clientHeight);
				}
			});

			context?.ContainerElement.addEventListener("hide.bs.modal", () => {
				if (new UAParser().getDevice().type === "mobile") {
					modalContentElementRef.current!.style.transform = "translateY(100%)";
				}
			});

			context?.ContainerElement.addEventListener("hidden.bs.modal", () => {
				modalContentElementRef.current!.style.transform = "";
				modalContentElementRef.current!.style.height = "";
			});

		}


	}, [context?.ContainerElement, modalContentElementRef]);

	const onTouchStart = (event: React.TouchEvent<HTMLDivElement>) => {
		setTouchYStart(event.touches[0].clientY)
	};

	const onTouchMove = (event: React.TouchEvent<HTMLDivElement>) => {
		if (touchYStart <= 0)
			return;

		var touchYEnd = event.changedTouches[0].clientY;
		var touchYDelta = touchYEnd - touchYStart;
		var touchYDirection = touchYDelta > 0 ? "down" : "up";

		const currentHeight = modalContentElementRef.current!.style.height
			? parseFloat(/[\d.]+/.exec(modalContentElementRef.current!.style.height)![0])
			: contentOriginalHeight;
		if (touchYDirection === "up") {
			// var heightChange = currentHeight - contentOriginalHeight;
			var upAcceleration = Math.abs(touchYDelta) / 8;
			touchYDelta = upAcceleration * -1;
			// if (currentHeight + touchYDelta > contentOriginalHeight) {
			// }
			// if (touchYDelta < -60) {
			// 	modalContentElementRef.current!.style.height = contentOriginalHeight + "px";
			// 	return;
			// }

			modalContentElementRef.current!.style.height = contentOriginalHeight + Math.abs(touchYDelta) + "px";
			modalContentElementRef.current!.style.transform = `translateY(0px)`;
		} else if (touchYDirection === "down") {
			var downAcceleration = Math.abs(touchYDelta) - Math.abs(touchYDelta / 1.8);
			touchYDelta = downAcceleration;
			if (currentHeight + touchYDelta < contentOriginalHeight) {
			}
			if (currentHeight === contentOriginalHeight) {
				modalContentElementRef.current!.style.height = contentOriginalHeight + "px";
				modalContentElementRef.current!.style.transform = `translateY(${touchYDelta + "px"})`;
			}
		}
	};

	const onTouchEnd = (event: React.TouchEvent<HTMLDivElement>) => {
		if (modalHeaderElementRef.current!.scrollHeight === modalHeaderElementRef.current!.clientHeight || modalHeaderElementRef.current!.scrollTop === 0) {
			if (event.changedTouches) {
				var touchYEnd = event.changedTouches[0].clientY;
				var touchYDelta = touchYEnd - touchYStart;
				var touchYDirection = touchYDelta > 0 ? "down" : "up";
				if (touchYDirection === "down") {
					const clientHeight = modalHeaderElementRef.current!.clientHeight;
					if (touchYDelta >= clientHeight / 3) {
						context!.Modal.hide();
						return;
					}

					modalContentElementRef.current!.style.transform = "";
				}
			}
		}

		modalHeaderElementRef.current?.dispatchEvent(new Event("touchcancel", { "bubbles": true }));
	};

	const onTouchCancel = (event: React.TouchEvent<HTMLDivElement>) => {
		setTouchYStart(0)
		modalContentElementRef.current!.style.transform = "translateY(0)";
		modalContentElementRef.current!.style.height = `${contentOriginalHeight}px`;
		// setTimeout(() => {
		modalContentElementRef.current!.style.transform = "";
		// 	modalContentElementRef.current!.style.height = "";
		// }, transitionDuration.current);
	};

	useEffect(() => {
		if (context?.ContainerElement && context?.Modal) {
			setContentElement(context.ContainerElement.querySelector(".modal-content") as HTMLDivElement)
			console.debug("Popup Header Updated.", context);
			var _comp = window.getComputedStyle(modalContentElementRef.current!).transitionDuration;
			if (_comp) {
				var _values = _comp.split(",");
				if (_values.length > 0) {
					var _value = /[\d.]+/.exec(_values[0].trim());
					if (_value && _value.length > 0) {
						transitionDuration.current = parseFloat(_value[0]) * 1000;
						console.debug("Popup Transition Duration:", transitionDuration.current);

					}
				}
			}
		}
	}, [context, context!.ContainerElement, context!.Modal]);

	var showCloseHtml: JSX.Element | null = null;
	if (props.showClose) {
		showCloseHtml = <button type="button" className="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>;
	}

	return (
		<div className="modal-header" ref={modalHeaderElementRefHandler} onTouchMove={onTouchMove} onTouchStart={onTouchStart} onTouchEnd={onTouchEnd} onTouchCancel={onTouchCancel}>
			<h5 className="modal-title">{props.title}</h5>
			{showCloseHtml}
		</div>
	);
};

export class Body extends Component<IBodyProps, {}> {
	render() {
		return <div className="modal-body">
			{this.props.children}
		</div>
	}
}

export class Footer extends Component<IFooterProps, IFooterState> {
	static contextType = PopupContext;
	context!: React.ContextType<typeof PopupContext>;

	constructor(props: IFooterProps) {
		super(props);

		this.state = {
			showClose: this.props.showClose || false,
			closeText: this.props.closeText || "Close",
		}
	}

	componentDidMount(): void {

		console.debug("Popup Footer Mounted.");
		if (this.context) {
			var footer = (this.props.children && this.props.children.length > 0) || this.props.showClose;
			this.context.hasFooter = footer;
		}

	}

	componentDidUpdate(prevProps: Readonly<IFooterProps>, prevState: Readonly<IFooterState>, snapshot?: any): void {
		_.isEqual(prevState, this.state) || console.debug("Popup Footer Updated.", this.state);
	}

	showCloseButton() {
		if (this.state.showClose)
			return <button type="button" className="btn btn-secondary" data-dismiss="modal">{this.state.closeText}</button>;
	}

	render() {
		if (!this.props.children && !this.state.showClose) {
			return <></>
		}

		var close = null;
		if (this.state.showClose)
			close = <button type="button" className="btn btn-secondary" data-bs-dismiss="modal">{this.state.closeText}</button>;

		return <div className="modal-footer">
			{close}
			{this.props.children}
		</div>
	}
}